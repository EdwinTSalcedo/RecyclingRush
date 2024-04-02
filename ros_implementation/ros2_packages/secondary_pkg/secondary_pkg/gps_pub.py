#!/usr/bin/env python3
import rclpy # Python library for ROS 2
from rclpy.node import Node # Handles the creation of nodes

from std_msgs.msg import String
from interfaces.msg import Gps 

import serial

class GpsPublisher(Node):

    def __init__(self):
        super().__init__('gps_publisher')

        # Initializing Topic
        self.publisher_ = self.create_publisher(Gps, '/gps_data', 10)  
        self.timer_period = 0.5  # seconds
        self.timer = self.create_timer(self.timer_period, self.timer_callback)
        self.serial_port = serial.Serial('/dev/ttyACM1', 9600) 

    def timer_callback(self):
        try:
            # Reading Serial Data
            data = self.serial_port.readline().decode('utf-8').strip()

            # Checking '$GNRMC' 
            if data.startswith("$GNRMC"):
                # Splitting Data
                fields = data.split(',')

                # Checking enough fields
                if len(fields) >= 12 and fields[2] == 'A':
                    # Extracting Data
                    latitude = float(fields[3][:2]) + float(fields[3][2:]) / 60
                    if fields[4] == 'S':
                        latitude = -latitude
                    longitude = float(fields[5][:3]) + float(fields[5][3:]) / 60
                    if fields[6] == 'W':
                        longitude = -longitude

                    # Publishing Topic Values
                    gps_msg = Gps()
                    gps_msg.latitude = latitude
                    gps_msg.longitude = longitude
                    self.publisher_.publish(gps_msg)
                    self.get_logger().info(f'Latitude: {latitude:.6f}, Longitude: {longitude:.6f}')

        except KeyboardInterrupt:
            self.serial_port.close()

def main(args=None):
    rclpy.init(args=args)

    gps_publisher = GpsPublisher()

    rclpy.spin(gps_publisher)

    # Destroy the node explicitly
    gps_publisher.destroy_node()
    rclpy.shutdown()

if __name__ == '__main__':
    main()

