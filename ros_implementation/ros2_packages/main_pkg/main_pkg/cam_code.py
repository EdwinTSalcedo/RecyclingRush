#!/usr/bin/env python3

import rclpy # Python library for ROS 2
from rclpy.node import Node # Handles the creation of nodes
from cv_bridge import CvBridge # Package to convert between ROS and OpenCV Images
import cv2 # OpenCV library
import numpy as np

from sensor_msgs.msg import Image # Image is the message type
from oakd_camera.msg import Yolo # Model is the message type
 
class ImageSubscriber(Node):
    
    def __init__(self):
        # Initiate the Node class's constructor and give it a name
        super().__init__('image_subscriber')
            
        # Create the subscriber. This subscriber will receive an Image
        self.oakd_cam_sub = self.create_subscription(
            Image, 
            '/oakd/camera', 
            self.oakd_cam_callback, 
            10)
        self.oakd_cam_sub # prevent unused variable warning

        # Create the subscriber. This subscriber will receive a Model
        self.oakd_model_sub = self.create_subscription(
            Yolo, 
            '/oakd/model', 
            self.oakd_model_callback, 
            10)
        self.oakd_model_sub # prevent unused variable warning

        # To save Image
        self.image = None
        self.image # prevent unused variable warning

        # To save Model
        self.model = Yolo()

        # Used to convert between ROS and OpenCV images
        self.bridge = CvBridge()

    def oakd_cam_callback(self, data):
        # Display the message on the console
        self.get_logger().info('Receiving video frame')

        # Convert ROS Image message to OpenCV image
        self.image = self.bridge.imgmsg_to_cv2(data)        

        # Actions to be done
        self.actions()

    def oakd_model_callback(self, data):
        # Display the message on the console
        self.get_logger().info('Receiving model data')

        # Saving Data
        self.model = data

    def actions(self):
        # Image Paramaters
        new_image = self.image
        color = (255,255,0)

        if(self.model.label.det1 != ''):
            self.draw_bbox(new_image, self.model.label.det1, self.model.confidence.det1, self.model.bboxes.det1, color)
        if(self.model.label.det2 != ''):
            self.draw_bbox(new_image, self.model.label.det2, self.model.confidence.det2, self.model.bboxes.det2, color)
        if(self.model.label.det3 != ''):
            self.draw_bbox(new_image, self.model.label.det3, self.model.confidence.det3, self.model.bboxes.det3, color)
        if(self.model.label.det4 != ''):
            self.draw_bbox(new_image, self.model.label.det4, self.model.confidence.det4, self.model.bboxes.det4, color)
        if(self.model.label.det5 != ''):
            self.draw_bbox(new_image, self.model.label.det5, self.model.confidence.det5, self.model.bboxes.det5, color)     
    
        # Display image
        cv2.imshow('Camera Resized', new_image)
        cv2.waitKey(1)

    def draw_bbox(self, image, label, confidence, bbox, color):
        cv2.putText(image, label, (bbox.bbox1+10, bbox.bbox2+20), cv2.LINE_AA, 0.8, color)
        cv2.putText(image, '{}%'.format(confidence), (bbox.bbox1+10, bbox.bbox2+40), cv2.LINE_AA, 0.8, color)
        cv2.rectangle(image, (bbox.bbox1, bbox.bbox2), (bbox.bbox3, bbox.bbox4), color, 2)


def main(args=None):
    # Initialize the rclpy library
    rclpy.init(args=args)

    # Create the node
    image_subscriber = ImageSubscriber()

    # Spin the node so the callback function is called.
    rclpy.spin(image_subscriber)

    # Destroy the node explicitly
    image_subscriber.destroy_node()

    # Shutdown the ROS client library for Python
    rclpy.shutdown()
  
if __name__ == '__main__':
    main()
