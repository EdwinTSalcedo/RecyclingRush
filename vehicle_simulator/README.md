Cross-View Gait Recognition Based on U-Net
Last Updated: December 19th, 2023

Table of Contents
Description | Features | Installation | Usage | Contributions | License | Contact
Additional Images
CAD Design Simulator

Left: CAD Design. Right: Simulator.

Description
This project is an aquatic simulator developed in Unity with the primary goal of simulating an underwater environment where a virtual agent collects water lentils. The simulator incorporates advanced features, such as the use of external Python servers for integrating object detection models based on YOLOv5 and ResNet. Additionally, stereo cameras are utilized to enhance the agent's perception of the environment.

Features
Water Lentil Collection: The virtual agent is designed to efficiently collect water lentils in a simulated aquatic environment.

Integration of Detection Models: External Python servers are employed to integrate object detection models based on YOLOv5 and ResNet. This enables the agent to detect and react to objects in its surroundings.

Stereo Cameras: A stereo camera setup is implemented to improve the three-dimensional perception of the environment and facilitate decision-making for the agent.

Installation
System Requirements
Unity
Python
Installation Steps
Clone this repository to your local machine:

git clone https://github.com/your_username/repository-name.git
cd repository-name
Configure the Unity environment to load the project.

Install Python dependencies for the detection models:

pip install -r requirements.txt
Inspect Subfolders:
RecyclingRush: Use the pre-packaged simulator by running the executable file inside this folder.

Robot_acuatico_autonomo: Open this folder in Unity for modification.

servers: Contains Python models; no further action needed during this installation step.

Initialize Simulator:
Run the pre-packaged simulator (.exe) located in the "RecyclingRush" folder.
This will create a folder named "!RecyclingRush" in your "Documents" directory.
Copy Servers Folder:
Inside the newly created "!RecyclingRush" folder, copy the entire "servers" folder.
This step is crucial for both the simulator and the program to utilize the Python models.
Unity Project Setup:
Open Unity and load the Unity project by navigating to the "Robot_acuatico_autonomo" folder.
If the project does not automatically initialize, follow these steps:
Click on "File" in the Unity editor.
Choose "Open Project."
Navigate to the "Robot_acuatico_autonomo" folder and open the project.
Now, you have the Unity project ready for modification.
Usage
Simulator:
Run the pre-packaged simulator (.exe) from the "RecyclingRush" folder to interact with the virtual environment.
Python Models:
The Python models in the "servers" folder are used for water lentil and obstacle detection by the simulator.
Unity Project:
Open and modify the Unity project located in the "Robot_acuatico_autonomo" folder according to your requirements.
Additional Notes
Make sure your system meets the requirements for running the simulator and Python models.

Customize the Unity project as needed, taking into account environmental constraints and the power of your computer for ride performance.

For any questions or problems, contact the development team using the contact information provided in the README.

Contributions
If you wish to contribute to this project, please follow these guidelines:

Fork the repository.

Create a branch for your contribution:

git checkout -b feature/new-feature
Develop and test your changes.

Submit a pull request to the main branch of the original repository.

License
This project is licensed under the [License Name]. Refer to the LICENSE file for more details.

Contact
For questions or comments, you can reach out to the development team:

Developer Name: [Name]
Email: [email@example.com]
