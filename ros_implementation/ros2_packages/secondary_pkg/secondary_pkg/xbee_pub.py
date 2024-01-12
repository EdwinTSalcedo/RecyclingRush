#!/usr/bin/env python3

import rclpy # Python library for ROS 2
from rclpy.node import Node # Handles the creation of nodes
import serial
import numpy as np

# Custom MSG Interfaces
from xbee_interfaces.msg import GpsControl
from xbee_interfaces.msg import Joystick
from xbee_interfaces.msg import Switch

# Publishing to cmd_vel
from geometry_msgs.msg import Twist

class XbeePublisher(Node):

    def __init__(self):
        # Initiate the Node class's constructor and give it a name
        super().__init__('xbee_publisher')

        # Initializing Serial Port
        self.ser = serial.Serial('/dev/ttyUSB0', 9600)
        self.ser.timeout = 0.5
        self.get_logger().info('Connected to: ' + self.ser.portstr)

        # Create Publishers
        self.gps_ = self.create_publisher(GpsControl, '/xbee/gps', 10)  
        self.joystick_ = self.create_publisher(Joystick, '/xbee/joystick', 10)  
        self.switch_ = self.create_publisher(Switch, '/xbee/switch', 10)  
        self.cmdvel_ = self.create_publisher(Twist, '/cmd_vel', 10)
        
        # Publish message every 0.1 seconds
        timer_period = 0.1  

        # Create the timer
        self.timer = self.create_timer(timer_period, self.timer_callback)

    def timer_callback(self):
        # Reading Data
        data = str(self.ser.readline())[2:-3]
        self.get_logger().info(data)

        # Decoding Data
        new_data = data.split('-')

        # Create Variable for cmd_vel
        aux_cmdvel = Twist()

        # Declaring Variables to Publish
        gps = GpsControl()
        joystick = Joystick()
        switch = Switch()

        if(len(data) > 25):            
            # Writing Data to Variables
            switch.sw1 = int(new_data[0][1:])
            switch.sw2 = int(new_data[1][1:])
            switch.sw3 = int(new_data[2][1:])
            joystick.x = int(new_data[3][1:])
            joystick.y = int(new_data[4][1:])

            try:
                gps.latitude = float(new_data[5][3:])
                gps.longitude = float(new_data[6][3:])

            except:
                gps.latitude = 0.0
                gps.longitude = 0.0
                pass
            # Writing cmd_vel data
            aux1 = 1.15 * (2 * np.clip(joystick.x, 0, 1024) - 1024)
            aux2 = 1.15 * (2 * (1024 - np.clip(joystick.y, 0, 1024)) - 1024)
            pwm_val = round(np.clip(np.sqrt(np.power(aux1, 2) + np.power(aux2, 2)), 0, 1000)/1000, 3)
            pwm_angle = np.degrees(np.arctan2(aux2, aux1))

            #self.get_logger().info('VAL: {}, ANGLE: {}'.format(round(pwm_val,2),round(pwm_angle,2)))

            if(45 < pwm_angle <= 135): 
                aux_cmdvel.linear.x = pwm_val
                aux_cmdvel.angular.z = 0.0
            if(135 < pwm_angle <= 180) or (-180 <= pwm_angle <= -135): 
                aux_cmdvel.linear.x = 0.0
                aux_cmdvel.angular.z = -pwm_val
            if(-135 < pwm_angle <= -45): 
                aux_cmdvel.linear.x = -pwm_val
                aux_cmdvel.angular.z = 0.0
            if(-45 < pwm_angle <= 45): 
                aux_cmdvel.linear.x = 0.0
                aux_cmdvel.angular.z = pwm_val

            # Flag for Correct Data
            self.get_logger().info('Publishing New Data !') 

        else:
            # Writing Aux Data to Variables
            switch.sw1 = 0
            switch.sw2 = 0
            switch.sw3 = 0
            joystick.x = 500
            joystick.y = 500
            gps.latitude = 0.000000
            gps.longitude = 0.000000
            
            # Writing cmd_vel data
            aux_cmdvel.linear.x = 0.0
            aux_cmdvel.angular.z = 0.0

            # Flag for Correct Data
            self.get_logger().info('Data is Incorrect !') 

        self.gps_.publish(gps)
        self.joystick_.publish(joystick)
        self.switch_.publish(switch)
        self.cmdvel_.publish(aux_cmdvel)
        

def main(args=None):
    # Initialize the rclpy library
    rclpy.init(args=args)

    # Create the node
    xbee_publisher = XbeePublisher()

    # SPin the node for calling the callback
    rclpy.spin(xbee_publisher)

    # Destroy the node
    xbee_publisher.destroy_node()

    # Shutdown the ROS client library for Python
    rclpy.shutdown()

if __name__ == '__main__':
    main()
