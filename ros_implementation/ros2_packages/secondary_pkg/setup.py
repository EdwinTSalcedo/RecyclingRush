from setuptools import setup

package_name = 'secondary_pkg'

setup(
    name=package_name,
    version='0.0.0',
    packages=[package_name],
    data_files=[
        ('share/ament_index/resource_index/packages',
            ['resource/' + package_name]),
        ('share/' + package_name, ['package.xml']),
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
        	'xbee_talker = secondary_pkg.xbee_pub:main',
            'oakd_talker = secondary_pkg.oakd_pub:main',
            'gps_talker = secondary_pkg.gps_pub:main',
            'model_talker = secondary_pkg.model_pub:main'
        ],
    },
)
