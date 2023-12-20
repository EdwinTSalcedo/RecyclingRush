# Cross-View Gait Recognition Based on U-Net

Last Updated: April 27th, 2021

## Table of Contents
<div align="center" style="background-color: #6495ED; padding: 10px;">
  <a href="#description" style="color: #FF00FF;"><b>Description</b></a> |
  <a href="#features" style="color: #FF00FF;"><b>Features</b></a> |
  <a href="#installation" style="color: #FF00FF;"><b>Installation</b></a> |
  <a href="#usage" style="color: #FF00FF;"><b>Usage</b></a> |
  <a href="#contributions" style="color: #FF00FF;"><b>Contributions</b></a> |
  <a href="#license" style="color: #FF00FF;"><b>License</b></a> |
  <a href="#contact" style="color: #FF00FF;"><b>Contact</b></a>
</div>

## Additional Images

<p align="center">
  <img src="Images/CAD.png" alt="CAD Design" width="400" style="margin-right: 20px;"/>
  <img src="Images/Sim.png" alt="Simulator" width="400"/>
</p>

## Título

![hasgdiasudiashduashdasdhas](cad_design.png)

<p align="center">
  <i>Left: CAD Design. Right: Simulator.</i>
</p>


## Description
This project is an aquatic simulator developed in Unity with the primary goal of simulating an underwater environment where a virtual agent collects water lentils. The simulator incorporates advanced features, such as the use of external Python servers for integrating object detection models based on YOLOv5 and ResNet. Additionally, stereo cameras are utilized to enhance the agent's perception of the environment.

## Features
- **Water Lentil Collection:** The virtual agent is designed to efficiently collect water lentils in a simulated aquatic environment.

- **Integration of Detection Models:** External Python servers are employed to integrate object detection models based on YOLOv5 and ResNet. This enables the agent to detect and react to objects in its surroundings.

- **Stereo Cameras:** A stereo camera setup is implemented to improve the three-dimensional perception of the environment and facilitate decision-making for the agent.

## Installation
### System Requirements
- [Unity](https://unity.com/)
- [Python](https://www.python.org/)

### Installation Steps
1. Clone this repository to your local machine:

    ```bash
    git clone https://github.com/your_username/repository-name.git
    cd repository-name
    ```

2. Configure the Unity environment to load the project.

3. Install Python dependencies for the detection models:

    ```bash
    pip install -r requirements.txt
    ```

### Inspect Subfolders:

- **RecyclingRush:** Use the pre-packaged simulator by running the executable file inside this folder.

- **Robot_acuatico_autonomo:** Open this folder in Unity for modification.

- **servers:** Contains Python models; no further action needed during this installation step.

### Initialize Simulator:

1. Run the pre-packaged simulator (.exe) located in the "RecyclingRush" folder.
2. This will create a folder named "!RecyclingRush" in your "Documents" directory.

### Copy Servers Folder:

1. Inside the newly created "!RecyclingRush" folder, copy the entire "servers" folder.
2. This step is crucial for both the simulator and the program to utilize the Python models.

### Unity Project Setup:

1. Open Unity and load the Unity project by navigating to the "Robot_acuatico_autonomo" folder.
2. If the project does not automatically initialize, follow these steps:
    - Click on "File" in the Unity editor.
    - Choose "Open Project."
    - Navigate to the "Robot_acuatico_autonomo" folder and open the project.
3. Now, you have the Unity project ready for modification.

## Usage

### Simulator:

- Run the pre-packaged simulator (.exe) from the "RecyclingRush" folder to interact with the virtual environment.

### Python Models:

- The Python models in the "servers" folder are used for water lentil and obstacle detection by the simulator.

### Unity Project:

- Open and modify the Unity project located in the "Robot_acuatico_autonomo" folder according to your requirements.

## Additional Notes

- Ensure that your system meets the requirements for running the simulator and Python models.

- Customize the Unity project as needed, considering the environmental variables and parameters defined within the project.

- For any questions or issues, contact the development team using the provided contact information in the README.


## Contributions
If you wish to contribute to this project, please follow these guidelines:

1. Fork the repository.

2. Create a branch for your contribution:

    ```bash
    git checkout -b feature/new-feature
    ```

3. Develop and test your changes.

4. Submit a pull request to the main branch of the original repository.

## License
This project is licensed under the [License Name]. Refer to the LICENSE file for more details.

## Contact
For questions or comments, you can reach out to the development team:

- **Developer Name:** [Name]
- **Email:** [email@example.com]
