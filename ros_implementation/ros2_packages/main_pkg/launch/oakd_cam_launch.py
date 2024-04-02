#!/usr/bin/env python3

import launch_ros.actions
from launch import LaunchDescription

def generate_launch_description():
    # Importing Code for Showing Camera
    oakd_show = launch_ros.actions.Node(
            package='secondary_pkg', 
            executable='oakd_talker',
            name='oakd_talker')
    
    # Packages to be Launched
    ld = LaunchDescription()
    ld.add_action(oakd_show)

    return ld
