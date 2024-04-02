#!/usr/bin/env python3
import os
from glob import glob
from setuptools import setup

package_name = 'main_pkg'

setup(
    name=package_name,
    version='0.0.0',
    packages=[package_name],
    data_files=[
        ('share/ament_index/resource_index/packages',
            ['resource/' + package_name]),
        ('share/' + package_name, ['package.xml']),
        (os.path.join('share', package_name), glob('launch/*launch.[pxy][yma]*'))
    ],
    install_requires=['setuptools'],
    zip_safe=True,
    maintainer='misael',
    maintainer_email='misael@todo.todo',
    description='TODO: Package description',
    license='TODO: License declaration',
    tests_require=['pytest'],
    entry_points={
        'console_scripts': [
        	'camera_sub = main_pkg.cam_code:main',
            'modality1_pub = main_pkg.modality_1:main',
            'modality2_pub = main_pkg.modality_2:main',
            'modality3_pub = main_pkg.modality_3:main',
            'depth_modality_pub = main_pkg.depth_modality:main'
        ],
    },
)
