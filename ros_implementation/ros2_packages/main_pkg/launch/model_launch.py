#!/usr/bin/env python3

import launch_ros.actions
from launch import LaunchDescription

def generate_launch_description():
    # Importing Code for Model Data
    model_show = launch_ros.actions.Node(
            package='secondary_pkg', 
            executable='model_talker',
            name='model_talker')
    
    # Packages to be Launched
    ld = LaunchDescription()
    ld.add_action(model_show)

    return ld
