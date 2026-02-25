# HitboxViewer

<p align="center">
  ⚒️ Universal <a href="https://github.com/BepInEx/BepInEx">BepInEx 5</a> plugin for Unity games ⚒️
</p>
<p align="center">
  Visualize hitboxes in real time  
</p>
<p align="center">
  ⭐ UI powered by <a href="https://github.com/sinai-dev/UniverseLib">UniverseLib</a> ⭐
</p>


## Showcasing

<p align="center">
  <img src="https://github.com/Rostmoment/HitboxViewer/blob/master/Showcasing/Menu.png?raw=true" width="700">
</p>

<p align="center">
  <img src="https://github.com/Rostmoment/HitboxViewer/blob/master/Showcasing/bbplus1.png?raw=true" width="700">
</p>

<p align="center">
  <img src="https://github.com/Rostmoment/HitboxViewer/blob/master/Showcasing/bbplus2.png?raw=true" width="700">
</p>

<p align="center">
  <img src="https://github.com/Rostmoment/HitboxViewer/blob/master/Showcasing/deltatraveler1.png?raw=true" width="700">
</p>

<p align="center">
  <img src="https://github.com/Rostmoment/HitboxViewer/blob/master/Showcasing/deltatraveler2.png?raw=true" width="700">
</p>

## Supported Components

HitboxViewer supports the following Unity components:
- `BoxCollider`
- `SphereCollider`
- `CapsuleCollider`
- `MeshCollider`
- `CharacterController`
- `CircleCollider2D`
- `BoxCollider2D`
- `NavMeshObstacle` (box and capsule)
  
The term **"hitbox"** refers to **any of these components** or its bound

## Configuration

Configuration file can be found by path `BepInEx\config\rost.moment.unity.hitboxviewer.cfg`


General configuration:
- `Menu Alpha` - No opacity of hitbox viewer menu
- `Toggle Key` - Key to show/hide hitbox viewer menu, can be any [KeyCode]("https://docs.unity3d.com/6000.3/Documentation/ScriptReference/KeyCode.html")
- `Startup Delay` - Number of seconds to wait before initializing hitbox viewer. Adjust it if you experience issues
- `Shader Name` - Name of shader that will be used for coloring hitbox outlines. Try to change it if outline of hitboxes don't render
- `Update Rate` - Frames between each hitbox update
- `Hide On Startup` - Should menu be hidden on start?

Hitboxes configuration:
- `Start Color` - Start RGB color of hitbox gradient outline
- `End Color` - End RGB color of hitbox gradient outline
- `Key Bind` - Key bind to enable/disable all showcasing hitbox of this type (NOT IMPLEMENTED YET!)
- `Start Width` - Start width of hitbox outline
- `End Width` - End width of hitbox outline

Some hitboxes types have unique for them configurations:
- `Draw algorithm` - what algorithm will be used for drawing hitbox of this type? (for SphereCollider, CapsuleCollider, NavMeshObstacle and CharacterController)
- `Points per unit` - Defines amount of points per unit, radius (and height for capsules) for rounded hitboxes. (for SphereCollider, CapsuleCollider, NavMeshObstacle, CharacterController and CircleCollider2D)

## Drawing rounded hitboxes algorithms

Hitbox viewer has 4 different algorithms to draw rounded hitboxes, some are more accurate, some are faster:
- `Latitude Longitude` - most accurate, but the slowest
- `Fibonacci` - faster than Latitude Longitude, but more noisy/scattered
- `Three Axis` - fast and clean, draws 3 circle outlines giving a clear sense of shape
- `Two Axis` - fastest, but only 2 circle outlines which may feel thin or incomplete for some shapes

## Flags system
Hitbox viewer uses a flags-based filtering system that allows you to enable or disable specific hitbox variations for each supported component type.

Each hitbox type can be filtered using bitwise flags.

Available Flags:
- `Trigger` - for `Collider` and `Collider2D`, shows colliders where `isTrigger` equals true
- `Not Trigger` - for `Collider` and `Collider2D`, shows colliders where `isTrigger` equals false
- `Box` - for `NavMeshObstacle`, shows hitboxes where shape is box
- `Capsule` - for `NavMeshObstacle`, shows hitboxes where shape is capsule

## How to use
1. Press toggle key (F4 by default)
2. In menu, select needed hitbox type
3. Enable needed flags
4. Done

## How to install
1. Install [BepInEx5]("https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.5")
2. Launch game to generate folder
3. Close game
4. Download hitbox viewer archive from release 
5. Drag `BepInEx` folder from it into game folder
6. Launch game
7. Done

