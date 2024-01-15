
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

Deficient domestic wastewater management, industrial waste, and floating debris are some leading factors that contribute to inland water pollution. The surplus of minerals and nutrients in overly contaminated zones can lead to the invasion of  floating weeds. *Lemnoideae*, commonly known as duckweed, is a family of floating plants that has no leaves or stems and forms dense colonies with a fast growth rate. If not controlled, duckweed establishes a green layer on the surface and depletes fish and other organisms of oxygen and sunlight.


Consequenly, we propose an open-source unmanned surface vehicle (USV) for automatic duckweed removal that costs less than $500. The USV uses 3D printed parts and common components that can be easily purchased. It is worth mentioning that the proposed approach won the First Global Prize in the [OpenCV AI Competition 2022](https://opencv.org/core-opencv/), organized by the [OpenCV Foundation](https://opencv.org/). Moreover, further prototyping details and testing results are available in the paper: 

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


# Citing

If you find our work useful in your project, please consider to cite the following paper. Give RecyclingRush a star ⭐ on GitHub and share it with your friends and colleagues. With your support, we can continue to innovate and push the boundaries of what's possible in water quality management research using autonomous vehicles.

```
@article{salcedo2024,
	author={Salcedo, Edwin and Uchani, Yamil and Mamani, Misael and Fernandez, Mariel},
	title={Towards Continuous Floating Invasive Plant Removal Using Unmanned Surface Vehicles and Computer Vision},
	journal={IEEE Access},
	year={2024},
	number={1},
	doi={10.1109/ACCESS.2024.3351764},
	url={https://doi.org/10.1109/ACCESS.2024.3351764}
}
```

