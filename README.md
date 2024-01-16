
![waving](https://capsule-render.vercel.app/api?type=waving&height=160&text=%20Recycling%20Rush&fontAlign=50&fontSize=30&fontAlignY=30&color=00629B&fontColor=FFFFFF) 

<p align="center">
<img src="images/main.gif" width="70%">
</p>

<div align="center">
  <a href="#Overview"><b>Overview</b></a> |
  <a href="#USV"><b>USV</b></a> |
  <a href="#SAE"><b>SAE</b></a> |
  <a href="#Simulator"><b>Simulator</b></a> |
  <a href="#Results"><b>Results</b></a> |
  <a href="#Citing"><b>Citing</b></a>
</div>

# Overview

Deficient domestic wastewater management, industrial waste, and floating debris are some leading factors that contribute to inland water pollution. The surplus of minerals and nutrients in overly contaminated zones can lead to the invasion of floating weeds. *Lemnoideae*, commonly known as duckweed, is a family of floating plants that has no leaves or stems and forms dense colonies with a fast growth rate. If not controlled, duckweed establishes a green layer on the surface and depletes fish and other organisms of oxygen and sunlight.


Consequently, we propose an open-source unmanned surface vehicle (USV) for automatic duckweed removal that costs less than $500. The USV uses 3D-printed parts and common components that can be easily purchased. It is worth mentioning that the proposed approach won the First Global Prize in the [OpenCV AI Competition 2022](https://opencv.org/core-opencv/), organized by the [OpenCV Foundation](https://opencv.org/). Moreover, further prototyping details and testing results are available in the paper: 

[Edwin Salcedo](https://www.linkedin.com/in/edwinsalcedo/), [Yamil Uchani](https://www.linkedin.com/in/yamiluchani), [Misael Mamani](https://www.linkedin.com/in/misaelmq680), and [Mariel Fernandez](https://www.linkedin.com/in/mariel-fernandez-baldivieso-7ba9a0263),
*Towards Continuous Floating Invasive Plant Removal Using Unmanned Surface Vehicles and Computer Vision*, IEEE Access 2024.

[[Paper]](https://ieeexplore.ieee.org/document/10385136) [[Video Abstract]](https://youtu.be/yTdTHCYgbhM) 

# USV

## CAD Design
![CAD DESIGN](https://media.giphy.com/media/UdUvtKM1NgC2zawLRk/giphy.gif)

This section contains the CAD designs for the Unmanned Surface Vehicle (USV) proposed for the collection of duckweed. It includes all the parts that were used to 3D print the vehicle, such as the "Control Box", "Main Body", "Floaters", and "Main Grid".

### Part Files
- [`Aux_1`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/Aux_1.SLDPRT): Support structures.
- [`Aux_2`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/Aux_2.SLDPRT): Support structures.
- [`Aux_3`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/Aux_3.SLDPRT): Support structures.
- [`BackSupport`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/BackSupport.SLDPRT): Used for the net grip that collects water lentils.
- [`BatteryCover`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/BatteryCover.SLDPRT): Covers the battery housing area.
- [`BoltHolder`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/BoltHolder.SLDPRT): Holder for bolts.
- [`BoxLid`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/BoxLid.SLDPRT): Lid for the control box.
- [`ElectronicBox`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/ElectronicBox.SLDPRT): Housing for the electronic components.
- [`Flipper`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/Flipper.SLDPRT): Grip for the motors.
- [`FloaterBase`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/FloaterBase.SLDPRT): Base structure for the floaters.
- [`FloaterLeftBack`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/FloaterLeftBack.SLDPRT): The left back part of the floaters.
- [`FloaterLeftFront`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/FloaterLeftFront.SLDPRT): The left front part of the floaters.
- [`FloaterRightBack`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/FloaterRightBack.SLDPRT): The right back part of the floaters.
- [`FloaterRightFront`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/FloaterRightFront.SLDPRT): The right front part of the floaters.
- [`FrontSupport`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/FrontSupport.SLDPRT): Frontal support structure.
- [`Grid`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/Grid.SLDPRT): Filtering grid.
- [`Joint`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/Joint.SLDPRT): Joint component.
- [`UpperStructure`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/UpperStructure.SLDPRT): Upper structure of the vehicle.
- [`VentLid`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/VentLid.SLDPRT): Lid for ventilation.


### Assembly File
- [`Assembly`](https://github.com/EdwinTSalcedo/RecyclingRush/blob/CAD_Design_USV/USV/Assembly.SLDASM): The complete assembly of the USV.

### Usage Instructions

The design was created in SolidWorks 2020, and it is recommended to use the same or a newer version for compatibility.

1. Download the desired part or assembly files from the repository.
2. Open the files using SolidWorks 2020 or a newer version.
3. To view or modify individual parts, open the part (.SLDPRT) files.
4. To view the complete assembly, open the `Assembly.SLDASM` file.

# SAE

## Framework

Architectural layout encapsulating the three most essential components:

- A duckweed detection model embedded inside an OAK-D camera. 
- A steering angle classification model and a temporal weight module, namely *momentum*. Both embedded inside a jetson nano development card.   

<p align="center">
<img src="images/graphical_abstract.jpg" width="70%">
</p>

## Duckweed detection

The database used for this model was built by scrapping images from Google Images and the [Global Biodiversity Information Facility](https://www.gbif.org/). The resulting 4,000 images were grouped into five categories according to the view-point of the duckweed colonies inside the samples: close, near-close, near-wide, wide, and empty. The final dataset is available [here](https://ieee-dataport.org/documents/duckweed-detection-dataset) (you'll need to create a IEEE Dataport account to access the dataset).

What follows is the list of experiments and trained models to replicate the results obtained using Yolov5 the enhanced versions of the dataset: 

#### Yolo v5

<table>
    <thead>
        <tr>
            <th>Dataset</th>
            <th>Weights</th>
            <th>IoU</th>
			<th>mAP</th>
			<th>Accuracy</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Only real</td>
			<td><a href='https://drive.google.com/file/d/1-WlrXWoTPu3yEuURRsOsNEjTxwbRDJd6/view?usp=sharing'>Gdrive</a></td>
			<td>0.7431</td>
			<td>0.6426</td>
			<td>0.7276</td>
        </tr>
        <tr>
			<td>Only virtual </td>
            <td><a href='https://drive.google.com/file/d/1-w_Ewk6iC9fOJJ1V7Cs3zVUpBro9w3ZQ/view?usp=drive_link'>Gdrive</a></td>
			<td>0.8191</td>
			<td>0.6487</td>
			<td>0.8092</td>
        </tr>
        <tr>
            <td>Real + Virtual</td>
			<td><a href='https://drive.google.com/file/d/1-sHAc9iJtIiQfX_DbvE2BOccPEIdxtKX/view?usp=sharing'>Gdrive</a></td>
			<td>0.8502</td>
			<td>0.878</td>
			<td>0.8327</td>
        </tr>
        <tr>
            <td>Real + Virtual + Data Augmentation</td>
			<td><a href='https://drive.google.com/file/d/1-cMYuxlzFK7b9STXs7TAoE3pCXD3WU7e/view?usp=sharing'>Gdrive</a></td>
			<td>0.8625</td>
			<td>0.866</td>
			<td>0.8472</td>
        </tr>
    </tbody>
</table>

#### Yolo v8

<table>
    <thead>
        <tr>
            <th>Dataset</th>
            <th>Weights</th>
            <th>IoU</th>
			<th>mAP</th>
			<th>Accuracy</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Only real</td>
			<td><a href='https://drive.google.com/file/d/1-kfFqMVYUzkwqlPPUIsO2SqbYUKPw2VY/view?usp=drive_link'>Gdrive</a></td>
			<td>0.8756</td>
			<td>0.7779</td>
			<td>0.8588</td>
        </tr>
        <tr>
			<td>Only virtual </td>
            <td><a href='https://drive.google.com/file/d/1DNfr5-64qjNELy2xFfFHN2N_WPpo_HfF/view?usp=drive_link'>Gdrive</a></td>
			<td>0.874</td>
			<td>0.6016</td>
			<td>0.8889</td>
        </tr>
        <tr>
            <td>Real + Virtual</td>
			<td><a href='https://drive.google.com/file/d/1-iBiTEx8Cba_mg1a6Kqz7WGS8SG289pw/view?usp=sharing'>Gdrive</a></td>
			<td>0.8085</td>
			<td>0.6903</td>
			<td>0.7878</td>
        </tr>
        <tr>
            <td><b>Real + Virtual + Data Augmentation</b></td>
			<td><a href='https://drive.google.com/file/d/1-R5eD4rBFjyCe5hp8DqntJTGVIdF3kmn/view?usp=sharing'>Gdrive</a></td>
			<td>0.8942</td>
			<td>0.7992</td>
			<td>0.8796</td>
        </tr>
    </tbody>
</table>

Our data preprocessing approach, as well as our Yolov5 and Yolov8 implementations can be found [here](https://github.com/EdwinTSalcedo/RecyclingRush/tree/master/duckweed_detection). 

## Steering angle classification

We tested two approaches for SAC: first, by collecting a syntethic dataset using a bespoke virtual environment and training DL classifiers. This approach, including object detection and moment, was named as *M3*. Then, we tried to classify steering angle using stereo vision along with object detection and momentum, which we later named *M4*. 

# Citing

If you find our work useful in your project, please consider to cite the following paper. Give RecyclingRush a star ⭐ on GitHub and share it with your friends and colleagues. With your support, we can continue to innovate in the field of autonomous vehicles for water quality management.

```
@article{salcedo2024,
  title={Towards Continuous Floating Invasive Plant Removal Using Unmanned Surface Vehicles and Computer Vision},
  author={Salcedo, Edwin and Uchani, Yamil and Mamani, Misael and Fernandez, Mariel},
  journal={IEEE Access},
  year={2024},
  publisher={IEEE},
  doi={10.1109/ACCESS.2024.3351764},
  url={https://doi.org/10.1109/ACCESS.2024.3351764}
}
```

