# SynthDataGen

**Synthetic Data Generation for Egocentric Hand-Object Interaction using Unity**

This project simulates egocentric hand-object interactions using Unity to generate diverse annotated data for training AI models.

It leverages the Unity Perception package and AutoHand VR Interaction asset to produce synthetic multimodal datasets with realistic hand-object interactions in randomized scenes.

The interacting objects are model truck components from SmartFactory, part of research conducted at DFKI Kaiserslautern.

## How It Works

This project simulates egocentric hand-object interactions using Unity to generate diverse annotated data for training AI models.

The system works through:
- Randomization: Scene elements like lighting, camera view, object placement, and textures are varied using Unity's built-in and custom randomizers.
- Interaction Logic: A coroutine (GrabReleaseCoroutine) simulates interactions using AutoHand’s Grab() and Release() methods. Interaction types are randomly chosen: 40% left-hand, 40% right-hand, 20% both.
- Data Output: The system automatically captures RGB images, 2D bounding boxes, instance masks, depth maps, and metadata—exported in SOLO (Synthetic Optimized Labeled Objects) format.

![Unity Scene](images/unity-scene.png)
![Dataset Sample](images/dataset-sample.png)
![Data Generation](images/data-generation.gif)

## Getting Started

1. Clone the repository  
   ```bash
   git clone https://github.com/hansika-s/SynthDataGen.git
   
2. Open the project using Unity 2022.3+
3. Install required Unity packages: Perception Package, AutoHand VR Interaction Toolkit(paid asset).
4. Open and run the main scene to start generating synthetic data.

## Acknowledgements
This project is inspired from the paper:
**Leonardi et al., 2024** — *Exploiting multimodal synthetic data for egocentric human-object interaction detection in an industrial scenario*  [DOI: 10.1016/j.cviu.2024.103984](https://doi.org/10.1016/j.cviu.2024.103984)

