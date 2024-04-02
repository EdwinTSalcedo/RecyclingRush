#!/usr/bin/env python3

import launch_ros.actions
from launch import LaunchDescription

def generate_launch_description():
    # Importing Code for Showing Camera
    xbee_show = launch_ros.actions.Node(
            package='secondary_pkg', 
            executable='xbee_talker',
            name='xbee_talker')
    
    # Packages to be Launched
    ld = LaunchDescription()
    ld.add_action(xbee_show)

    return ld
