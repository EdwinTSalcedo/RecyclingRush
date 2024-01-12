#!/usr/bin/env python3

import rclpy # Python library for ROS 2
from rclpy.node import Node # Handles the creation of nodes

from pathlib import Path
import cv2
from cv_bridge import CvBridge
import numpy as np
import torch
import torchvision

# Subscribing to Image
from sensor_msgs.msg import Image
from oakd_camera.msg import Model
from oakd_camera.msg import Yolo

class ModelPublisher(Node):
    def __init__(self):
        # Initiate the Node class's constructor and give it a name
        super().__init__('model_publisher')

        # Create the subscriber. This subscriber will receive an Image
        self.oakd_cam_sub = self.create_subscription(
            Image, 
            '/oakd/camera', 
            self.oakd_cam_callback, 
            10)
        self.oakd_cam_sub # prevent unused variable warning

        # Create the subscriber. This subscriber will receive a Model
        self.oakd_yolo_sub = self.create_subscription(
            Yolo, 
            '/oakd/yolo', 
            self.oakd_yolo_callback, 
            10)
        self.oakd_yolo_sub # prevent unused variable warning

        # Create Publishers
        self.model_ = self.create_publisher(Model, '/oakd/models', 10)  

        # Confifguring SAC
        self.steering_estimator = torchvision.models.resnet34()
        self.steering_estimator.fc = torch.nn.Linear(in_features=512, out_features=7)
        state_dict = torch.load("/home/misael/ros2_workspaces/recrush_ws/src/secondary_pkg/config/steering_estimator.pt" , map_location=torch.device('cpu'))
        self.steering_estimator.load_state_dict(state_dict)
        self.steering_estimator.eval()

        # To save Image
        self.image = None
        self.image # prevent unused variable warning

        # To save Model
        self.model = Model()

        # Used to convert between ROS and OpenCV images
        self.bridge = CvBridge()

    def oakd_cam_callback(self, data):
        # Display the message on the console
        self.get_logger().info('Receiving video frame')

        # Convert ROS Image message to OpenCV image
        self.image = self.bridge.imgmsg_to_cv2(data)  

        # Actions to be done
        self.actions()   

    def oakd_yolo_callback(self, msg):
        try:
            # Saving Bounding Boxes
            det1 = self.model.bboxes.det1
            det2 = self.model.bboxes.det2
            det3 = self.model.bboxes.det3
            det4 = self.model.bboxes.det4
            det5 = self.model.bboxes.det5

            # Average of Detections
            x_values = [(det1.bbox1+det1.bbox3)/2, (det2.bbox1+det2.bbox3)/2, (det3.bbox1+det3.bbox3)/2, (det4.bbox1+det4.bbox3)/2, (det5.bbox1+det5.bbox3)/2]
            mean_x = np.mean(np.array(x_values))
        except:
            mean_x = 640
        
        # Setting Final Value of Yolo Detection
        if(0 < mean_x < 182):
            self.model.yolo_angle = 0
        elif(182 < mean_x < 364):
            self.model.yolo_angle = 1
        elif(364 < mean_x < 546):
            self.model.yolo_angle = 2
        elif(546 < mean_x < 728):
            self.model.yolo_angle = 3
        elif(728 < mean_x < 910):
            self.model.yolo_angle = 4
        elif(910 < mean_x < 1092):
            self.model.yolo_angle = 5
        elif(1092 < mean_x < 1280):
            self.model.yolo_angle = 6

        
    def actions(self):
        # Image Paramaters
        new_image = cv2.resize(self.image, (224,126), interpolation = cv2.INTER_AREA)

        # Model
        compose = [torchvision.transforms.ToTensor(), torchvision.transforms.Normalize(mean=[0.485, 0.456, 0.406], std=[0.229, 0.224, 0.225])]
        transform = torchvision.transforms.Compose(compose)
        image_torch = transform(new_image)
        outputs = self.steering_estimator(torch.unsqueeze(image_torch, 0))
        _, preds = torch.max(outputs, 1)
        
        # Angle predicted by steering angle estimator
        self.model.sac_angle = preds.cpu().numpy()[0]
        

def main(args=None):
    # Initialize the rclpy library
    rclpy.init(args=args)

    # Create the node
    model_publisher = ModelPublisher()

    # Spin the node so the callback function is called.
    rclpy.spin(model_publisher)

    # Destroy the node explicitly
    model_publisher.destroy_node()

    # Shutdown the ROS client library for Python
    rclpy.shutdown()
  
if __name__ == '__main__':
    main()
