# Assignment 3: Antymology
This Repo creates a simulation of an ant colony. The ant colony uses a neural network to try to maximize the size of the nest. The colony consists of multiple ants and a single queen. The queen is the only ant that can create the nest. Each ant must go out and gather food and bring it to the queen. The ant learns how to maxmize the nest size by using a neural network. All ants share the same neural network and the network mutates after each generation.

The settings of the ant world can be found and changed in the world manager.

### Behaviour
The Behaviour component consists of The Ants behaviours, The Queens Behaviours and the Neural Network. The neural network uses a tanH activation method.
-My code implements a neural Network that all the ants share and use to determine there next move. 
- Each ant can perform 6 actions which are to :
        0 - move 1 tile in x axis
        1 - move 1 tile in z axis
        2 - move -1 tile in x axis
        3 - move -1 tile in z axis
        4 - eat mulch tile
        5 - dig up one tile
- The queen can perform 7 actions which are to :
        0 - move 1 tile in x axis
        1 - move 1 tile in z axis
        2 - move -1 tile in x axis
        3 - move -1 tile in z axis
        4 - eat mulch tile
        5 - dig up one tile
        6- Set a nestBlock
        
-The neural networks input layer uses the queens location and the ants health and input layers.
-The queen ant only sets nest blocks and then waits for ants to bring her food.
-The fitness is determined by how many nest blocks are placed. Looking back on this it might have made more sense to also take into consideration ant health as well.

### Configuration
This is the component responsible for configuring the system. For example, currently there exists a file called ConfigurationManager which holds the values responsible for world generation such as the dimensions of the world, and the seed used in the RNG. As you build parameters into your system, you will need to place your necesarry configuration components in here.

### Terrain
The terrain memory, generation, and display all take place in the terrain component. The main WorldManager is responsible for generating everything including the ants and queen.

### UI
 Currently consists of a fly camera, and a camera-controlled map editor and a basic GUI that displays how many nest blocks have been placed.

## Requirements
.
 - All project documentation should be provided via a Readme.md file found in your repo. Write it as if I was an employer who wanted to see a portfolio of your work. By that I mean write it as if I have no idea what the project is. Describe it in detail. Include images/gifs.

