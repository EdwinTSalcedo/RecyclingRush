#!/usr/bin/env python3

import rclpy # Python library for ROS 2
from rclpy.node import Node # Handles the creation of nodes
from pathlib import Path
import cv2
from cv_bridge import CvBridge
import depthai as dai
import numpy as np
import json
import blobconverter

# Custom MSG Interfaces
from oakd_camera.msg import Yolo
from oakd_camera.msg import BBoxes
from oakd_camera.msg import Box
from oakd_camera.msg import Label
from oakd_camera.msg import Confidence
from oakd_camera.msg import Depth

# Publishing to Image
from sensor_msgs.msg import Image

class OakDPublisher(Node):

    def __init__(self):
        # Initiate the Node class's constructor and give it a name
        super().__init__('oakd_publisher')

        # Parse Config (.json file)
        configPath = Path('/home/misael/ros2_workspaces/recrush_ws/src/secondary_pkg/config/best.json')
        if not configPath.exists():
            raise ValueError("Path {} does not exist!".format(configPath))

        with configPath.open() as f:
            config = json.load(f)
        nnConfig = config.get("nn_config", {})

        # Parse Input Size 
        if "input_size" in nnConfig:
            W, H = tuple(map(int, nnConfig.get("input_size").split('x')))

        # Extracting Metadata
        metadata = nnConfig.get("NN_specific_metadata", {})
        classes = metadata.get("classes", {})
        coordinates = metadata.get("coordinates", {})
        anchors = metadata.get("anchors", {})
        anchorMasks = metadata.get("anchor_masks", {})
        iouThreshold = metadata.get("iou_threshold", {})
        confidenceThreshold = metadata.get("confidence_threshold", {})

        # Parse Labels
        nnMappings = config.get("mappings", {})
        self.labels = nnMappings.get("labels", {})

        # get model path
        nnPath = '/home/misael/ros2_workspaces/recrush_ws/src/secondary_pkg/config/best_openvino_2021.4_6shave.blob'
        if not Path(nnPath).exists():
            print("No blob found at {}. Looking into DepthAI model zoo.".format(nnPath))
            nnPath = str(blobconverter.from_zoo('best_openvino_2021.4_6shave.blob', shaves = 6, zoo_type = "depthai", use_cache=True))
        # sync outputs
        syncNN = True

        # Creating Pipeline
        self.pipeline = dai.Pipeline()

        # Defining Sources and Inputs
        camRgb = self.pipeline.create(dai.node.ColorCamera)
        detectionNetwork = self.pipeline.create(dai.node.YoloDetectionNetwork)
        xoutRgb = self.pipeline.create(dai.node.XLinkOut)
        nnOut = self.pipeline.create(dai.node.XLinkOut)

        xoutRgb.setStreamName("rgb")
        nnOut.setStreamName("nn")

        # Camera Properties
        camRgb.setPreviewSize(W, H)
        camRgb.setResolution(dai.ColorCameraProperties.SensorResolution.THE_1080_P)
        camRgb.setInterleaved(False)
        camRgb.setColorOrder(dai.ColorCameraProperties.ColorOrder.BGR)
        camRgb.setFps(40)

        # Neural Network Settings
        detectionNetwork.setConfidenceThreshold(confidenceThreshold)
        detectionNetwork.setNumClasses(classes)
        detectionNetwork.setCoordinateSize(coordinates)
        detectionNetwork.setAnchors(anchors)
        detectionNetwork.setAnchorMasks(anchorMasks)
        detectionNetwork.setIouThreshold(iouThreshold)
        detectionNetwork.setBlobPath(nnPath)
        detectionNetwork.setNumInferenceThreads(2)
        detectionNetwork.input.setBlocking(False)

        # Linking Inputs and Outputs
        camRgb.preview.link(detectionNetwork.input)
        detectionNetwork.passthrough.link(xoutRgb.input)
        detectionNetwork.out.link(nnOut.input)

        # Printing Relevant Data
        self.get_logger().info('Classes: {}'.format(classes))
        self.get_logger().info('Labels: {}'.format(self.labels))
        self.get_logger().info('IoU Threshold: {}'.format(iouThreshold))
        self.get_logger().info('Confidence Threshold: {}'.format(confidenceThreshold))
        self.get_logger().info('===========================================')
        
        # Create Publishers
        self.model_ = self.create_publisher(Yolo, '/oakd/yolo', 10)  
        self.camera_ = self.create_publisher(Image, '/oakd/camera', 10)
        self.min_depth_ = self.create_publisher(Depth, '/oakd/depth_angle', 10)
        self.bridge = CvBridge()

        with dai.Device(self.pipeline) as device:
            # Output queues will be used to get the rgb frames and nn data from the outputs defined above
            self.qRgb = device.getOutputQueue(name="rgb", maxSize=4, blocking=False)
            self.qDet = device.getOutputQueue(name="nn", maxSize=4, blocking=False)
            self.qDepth = device.getOutputQueue(name="depth", maxSize=4, blocking=False)

            # Initializing Variables
            self.frame = None
            self.detections = []
            self.depth_data = None

            while (True):
                # Connecting to Device and Starting Pipeline
                inRgb = self.qRgb.get()
                inDet = self.qDet.get()
                inDepth = self.qDepth.get() 
                
                if inRgb is not None: 
                    self.frame = inRgb.getCvFrame()
                if inDet is not None: 
                    self.detections = inDet.detections
                if inDepth is not None:
                    self.depth_data = inDepth.getFrame()
                
                if self.frame is not None: 
                    self.displayFrame("RGB Image", self.frame, self.detections)
                if self.depth_data is not None:
                    self.displayDepth("Depth Data", self.depth_data)

                cv2.waitKey(1)

    # Normalization of Bounding Boxes
    def frameNorm(self, frame, bbox):
        normVals = np.full(len(bbox), frame.shape[0])
        normVals[::2] = frame.shape[1]
        return (np.clip(np.array(bbox), 0, 1) * normVals).astype(int)

    # Displaying Frame and Detections
    def displayFrame(self, name, frame, detections):
        # Aux Variables
        var_label = Label()
        var_bbox = BBoxes()
        var_confidence = Confidence()
        i = 1

        for detection in detections:
            # Bounding Box
            bbox = self.frameNorm(frame, (detection.xmin, detection.ymin, detection.xmax, detection.ymax))

            # Saving Bounding Box data
            var_box = Box()
            var_box.bbox1 = int(bbox[0])
            var_box.bbox2 = int(bbox[1])
            var_box.bbox3 = int(bbox[2])
            var_box.bbox4 = int(bbox[3])

            # Saving Data
            if(i == 1):
                var_label.det1 = str(self.labels[detection.label])
                var_bbox.det1 = var_box
                var_confidence.det1 = int(detection.confidence*100)
            elif(i == 2):
                var_label.det2 = str(self.labels[detection.label])
                var_bbox.det2 = var_box
                var_confidence.det2 = int(detection.confidence*100)
            elif(i == 3):
                var_label.det3 = str(self.labels[detection.label])
                var_bbox.det3 = var_box
                var_confidence.det3 = int(detection.confidence*100)
            elif(i == 4):
                var_label.det4 = str(self.labels[detection.label])
                var_bbox.det4 = var_box
                var_confidence.det4 = int(detection.confidence*100)
            elif(i == 5):
                var_label.det5 = str(self.labels[detection.label])
                var_bbox.det5 = var_box
                var_confidence.det5 = int(detection.confidence*100)

            i += 1

        # Saving Model Data
        var_model = Yolo()
        var_model.label = var_label
        var_model.confidence = var_confidence
        var_model.bboxes = var_bbox

        self.model_.publish(var_model)
        self.camera_.publish(self.bridge.cv2_to_imgmsg(frame))
    
    # Displaying Depth
    def displayDepth(self, depth_data):
        # Saving Depth Data
        depth = Depth()

        # Checking Depth Image
        height, width = depth_data.shape
        section = width // 7

        # Checking Max Distance for Collision Avoidance
        max_distance = 0
        max_section = 3

        for i in range(7):
            start = i * section
            end = start + section
            div_img = depth_data[:, start:end]
            max_section = np.max(div_img)

            if(max_section > max_distance):
                max_distance = max_section
                max_section = i


        depth.depth_angle = max_section

        # Publishing Data
        self.min_depth_.publish(depth)


def main(args=None):
    # Initialize the rclpy library
    rclpy.init(args=args)

    # Create the node
    oakd_publisher = OakDPublisher()

    # SPin the node for calling the callback
    rclpy.spin(oakd_publisher)

    # Destroy the node
    oakd_publisher.destroy_node()

    # Shutdown the ROS client library for Python
    rclpy.shutdown()

if __name__ == '__main__':
    main()
