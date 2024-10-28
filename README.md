# Procedural Map Creator in Unity using Perlin Noise

This project is focus on the creation of maps dynamically with the use of the Perlin Noise algorithm, it will explore and study its complexity step by step with the use of C++ and Unity (version 2021.3.10f1).

## Perlin Noise

When using random algorithms, machines tend to take numbers far from each other, if it uses them for example to build a graph it will result on a illegible and chaotic figure. When talking about "Noise" is important to take in count that it is also a random algorithm but with a step.

When talking more specifically about Perlin Noise, we are talking about an algorithm that when it takes a number at any time "t", it is related to its brother "number t + 1" and "number t - 1", resulting on a smoother graph, in a way, Perlin Noise can be considered a "Smooth Random Generator Algorithm".

<p align="center">
  <img src="Procedural-Map-Creator/Assets/Photos/RandomFunction.PNG" alt="Random Graph" width="45%" style="display:inline-block; margin-right: 10px;"/>
  <img src="Procedural-Map-Creator/Assets/Photos/PerlinNoise_3differentPerlin.PNG" alt="Perlin Noise Graph" width="45%" style="display:inline-block;"/>
</p>


To put more context on this topic, Ken Perlin, the original creator of this algorithm, created on the 1980's to help on the creation of textures of 3D objects on Tron, the idea was to create a "Procedural Texture Creator" that would take seconds to create something that artists would need days.

This can be helpful on different enviroments such as map creators, which can use the calculated values to establish mountain, valleys, etc. more naturally with less steep slopes. Now a days games like Mincraft use similar algorithms to create not only the terrain but the different biomes.

## Development

Lets go step by step developing the algorithm on different dimensions.

### 1 Dimension

When using the algorithm on 1D the numbers chosen range between 1 and -1, it is important to make a smooth interpolation (not linear) between this numbers, here Unity helps a lot with the development since it already incorporate its own interpolation.

Some important terms used when creating a Perlin Noise algorithm are: _Octaves_, _Frecuency_ and _Amplitude_. The last two are used regularly when talking about: functions, waves, etc. the first term _Octave_ refers to an existing Noise layer on the whole Perlin Noise value.

About the _Frecency_ and _Amplitude_ they can give different and interesting results depending on the values used, the _Frecuency_ is how much samples are taking within time, while the _Amplitude_ is refer to the maximun value that can be taken of the sample.

<p align="center">
  <img src="Procedural-Map-Creator/Assets/Photos/Line_100Points_1freq_1amp_0.1jump.PNG" alt="Frecuency and Amplitude values established to 1" width="30%" />
  <img src="Procedural-Map-Creator/Assets/Photos/Line_100Points_1freq_2amp_0.1jump.PNG" alt="Amplitude value established to 2" width="30%" />
  <img src="Procedural-Map-Creator/Assets/Photos/Line_100Points_2freq_1amp_0.1jump.PNG" alt="Frecuency value established to 2" width="30%" />
</p>

while this representations can be fascinating by themselves, they ca be fused to give even more complex Perlin Noise graphs, when doing this, it can be said that each one of those graphs are _Octaves_.

<p align="center">
  <img src="Procedural-Map-Creator/Assets/Photos/PerlinNoise_Sin.PNG" alt="Perlin Noise mixed with other functions" />
</p>

This _Octaves_ can be used, not only over other Perlin Noise but in other functions, in this example, Perlin Noise is being applide to a Sine one.

A curiosity about these Perlin Noise Graphs is the fact that they can be considered fractals, since the same pattern can be found over and over again regardless of how big, small or how many reference points are taken in the samples.

### 2 Dimensions

Before talking about Perlin Noise on 2 Dimensions there are some issues that need to be work on, such as the terrain.

Although Unity is a great engine to work around geometry, there are some limitations that can be fixed manually, one is the default plane, the main problem with this plane is the fact that it cannot be modify beyond some veriables such as the scale, position and rotation.

When handling this type of problems on geometry, variable such as vertices, are one of the most important thing to take in count, so in this type of problems creating your own plane is the optimal option.

## Creating a plane

As said before, when creating a plane, one of the most important things are the vertices, to understand better how a plane works in unity is important to know its design.

A plane is nothing more than a group of vertices joined by lines to form triangles, therefore in order to create one, it is necessary to establish how many vertices are wanted and its respective (plane local) positions. Once thay all are fixed the next step is to find a way to join each one of them with its respective neighbors in a certain order so that the plane can be render and behave correctly.

<p align="center">
  <img src="Procedural-Map-Creator/Assets/Photos/Vertices_and_Triangles_blue.PNG" alt="clockwise order 1" width="45%" style="display:inline-block; margin-right: 10px;"/>
  <img src="Procedural-Map-Creator/Assets/Photos/Vertices_and_Triangles_red.PNG" alt="clockwise order 2" width="45%" style="display:inline-block;"/>
</p>

when creating a triangles from a vertice it is important to choose a clockwise order as explained before, with this order the triangle will render and behave correctly.

<p align="center">
  <img src="Procedural-Map-Creator/Assets/Photos/Vertices_and_Triangles..PNG" alt="Plane of 2 triangles"/>
</p>

This example shows a very simple plane created by 2 triangles, if both (clockwise and counter clockwise) are made when creating the triangles a double-side render plane will be created, a better yet more expensive solution.

Other thing to take in count is the _definition_ of the plane, this _definition_ is used to measure how many vertices are gonna be needed to create the pleane, the more vertices the better the plane will look, knowing that it will also need to compute more values.

<p align="center">
  <img src="Procedural-Map-Creator/Assets/Photos/Planes_Definition.PNG" alt="Plane Definition" width="45%" style="display:inline-block; margin-right: 10px;"/>
  <img src="Procedural-Map-Creator/Assets/Photos/Planes_Definition_2.PNG" alt="Plane Definition 2" width="45%" style="display:inline-block;"/>
</p>


