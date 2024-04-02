#!/usr/bin/env python3

import rclpy # Python library for ROS 2
from rclpy.node import Node # Handles the creation of nodes

import math

# Importing Custom Interfaces
from geometry_msgs.msg import Twist
from oakd_camera.msg import Model
from interfaces import Gps

class Modality2(Node):
    def __init__(self):
        # Initiate the Node class's constructor and give it a name
        super().__init__('modality2_sub')

        # Create the subscriber. This subscriber will receive Yolo and SAC Angle
        self.model_sub = self.create_subscription(
            Model, 
            '/oakd/models', 
            self.models_callback, 
            10)
        self.model_sub # prevent unused variable warning

        # Create the subscriber. This subscriber will receive GPS Data
        self.gps_sub = self.create_subscription(
            Gps, 
            '/gps_data', 
            self.gps_callback, 
            10)
        self.model_sub # prevent unused variable warning

        # Create Publisher for Motion
        self.motion_ = self.create_publisher(Twist, '/cmd_vel', 10)  
        
        # Variables
        self.twist_msg = Twist()
        self.models_angles = Model()
        self.gps_data = Gps()
        self.final_angle = None

        # Variables for Modality 2
        self.inference_queue = []  # La cola para almacenar inferencias de ángulo
        self.momentum = 0.3        # El valor de momentum para el cálculo del ángulo
        self.max_queue_size = 10

    def get_final_angle(self):
        self.final_angle = self.models_angles.yolo_angle

    def checking_gps_range(self):
        # Center of Work
        lat_central = -16.5826664
        lon_central = -68.1568552
        
        # Conversion for Meters 
        delta_lat = 10 / 111139  
        delta_lon = 10 / (111139 * math.cos(math.radians(lat_central))) 

        # Rectangle
        lat_norte = lat_central + delta_lat
        lat_sur = lat_central - delta_lat
        lon_este = lon_central + delta_lon
        lon_oeste = lon_central - delta_lon

        return 1 if lat_sur <= self.gps_data.latitude <= lat_norte and lon_oeste <= self.gps_data.longitude <= lon_este else 0
    
    def models_callback(self, msg):
        # Saving Yolo Angle
        self.models_angles.yolo_angle = msg.yolo_angle

        # Display the message on the console
        self.get_logger().info('Yolo Angle Received :)')

        # Get Final Angle
        self.get_final_angle()

        # Actions to be done
        self.actions()  
    
    def gps_callback(self, msg):
        # Saving GPS Data
        self.gps_data = msg

        # Display the message on the console
        self.get_logger().info('GPS Data Received :)')

    def actions(self):
        # Checking Angles and GPS
        gps_check = self.checking_gps_range()

        if(gps_check == 1):
            # Adding Angle to Inference Queue
            self.inference_queue.insert(0, self.final_angle)

            # Calculating Next Angle
            next_angle = sum(element * (self.momentum ** i) for i, element in enumerate(self.inference_queue))

            # keeping Original Size
            if len(self.inference_queue) > self.max_queue_size:
                self.inference_queue.pop()

            # Rounding Next Angle
            next_angle = round(next_angle)

            # Usar el siguiente ángulo para las acciones
            self.final_angle = next_angle


            if(self.final_angle == 0):
                self.twist_msg.linear = 0
                self.twist_msg.angular = -1
            elif(self.final_angle == 1):
                self.twist_msg.linear = 0.5
                self.twist_msg.angular = -0.5
            elif(self.final_angle == 2):
                self.twist_msg.linear = 0.8
                self.twist_msg.angular = -0.2
            elif(self.final_angle == 3):
                self.twist_msg.linear = 1
                self.twist_msg.angular = 0
            elif(self.final_angle == 4):
                self.twist_msg.linear = 0.8
                self.twist_msg.angular = 0.2
            elif(self.final_angle == 5):
                self.twist_msg.linear = 0.5
                self.twist_msg.angular = 0.5
            elif(self.final_angle == 6):
                self.twist_msg.linear = 0
                self.twist_msg.angular = 1

        else:
            self.twist_msg.linear = -0.7
            self.twist_msg.angular = -0.5

        # Publishing Motion
        self.motion_.publish(self.twist_msg)


def main(args=None):
    # Initialize the rclpy library
    rclpy.init(args=args)

    # Create the node
    modality_2 = Modality2()

    # SPin the node for calling the callback
    rclpy.spin(modality_2)

    # Destroy the node
    modality_2.destroy_node()

    # Shutdown the ROS client library for Python
    rclpy.shutdown()

if __name__ == '__main__':
    main()