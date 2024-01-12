#!/usr/bin/env python3

import launch_ros.actions
from launch import LaunchDescription

def generate_launch_description():
    # Importing Code for GPS Data
    gps_show = launch_ros.actions.Node(
            package='secondary_pkg', 
            executable='gps_talker',
            name='gps_talker')
    
    # Packages to be Launched
    ld = LaunchDescription()
    ld.add_action(gps_show)

    return ld
