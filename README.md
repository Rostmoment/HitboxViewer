# HitboxViewer
# Main Information
 
Tool for viewing hitboxes in Unity games

[BepInEx](https://github.com/BepInEx/BepInEx) plugin that works with any Unity game

It uses LineRenderer for visualization hitboxes of most forms

Supports «Collider», «Collider2D» and «NavMeshObstacle» classes, by word «hitbox» I mean anything of them

HitboxViewer provides 6 different modes for displaying hitboxes (2 for colliders, 2 for navmeshes, 2 combined for both)
| Mode | For Collider? | For Collider2D? | For NavmeshObstacle? | Description |
| -----|:-------------:| :-------------: | :------------------: | ----------: |
| Hide | Yes ✅        | Yes ✅         | Yes ✅               | All colliders/navmeshes are hidden |   
| All  | Yes ✅        | Yes ✅         | Yes ✅               | All colliders/navmeshes are visible |   
| Trigger  | Yes ✅        | Yes ✅         | No ❌               | All colliders where propery «isTrigger» is true are visible |  
| Collider  | Yes ✅        | Yes ✅         | No ❌               | All colliders where propery «isTrigger» is false are visible |   
| Box  | No ❌       | No ❌        | Yes ✅               | All navmeshes that have box shape are visible |   
| Sphere  | No ❌       | No ❌        | Yes ✅               | All navmeshes that have capsule shape are visible |   

The display mode for navmeshes and colliders can be selected independently of each other
Press F1 for changing colliders displaying mode or F2 for navmeshes displaying mode

Supported hitbox types:
1. BoxCollider
2. SphereCollider
3. CapsuleCollider
4. TerrainCollider (maybe, I didn't test this one)
5. BoxCollider2D
6. CircleCollider2D
7. NavMeshObstacle Box
8. NavMeshObstacle Capsule
## If game throws exception (error) when trying to display hitbox, try to change shader name in configs (see below for instructions on how to do this). Shader name can be found using [UnityExplorer](https://github.com/sinai-dev/UnityExplorer)
# Showcasing
There are few screenshots of testing this tools
![Testing in Baldi's Basics Plus](https://github.com/Rostmoment/HitboxViewer/blob/master/Showcasing/baldiplus.png?Raw=true)
![Testing in Baldi's Basics Plus](https://github.com/Rostmoment/HitboxViewer/blob/master/Showcasing/baldiplus2.png?Raw=true)
![Testing in Baldi's Basics Plus](https://github.com/Rostmoment/HitboxViewer/blob/master/Showcasing/baldiplus3.png?Raw=true)
![Testing in Deltatraveler](https://github.com/Rostmoment/HitboxViewer/blob/master/Showcasing/deltatraveler.png?Raw=true)
![Testing in Deltatraveler](https://github.com/Rostmoment/HitboxViewer/blob/master/Showcasing/deltatraveler1.png?Raw=true)
![Testing in Deltatraveler](https://github.com/Rostmoment/HitboxViewer/blob/master/Showcasing/deltatraveler2.png?Raw=true)
As you can see, hitboxes are displaying with LineRenderer

# Configs
You can find and edit the configs of HitboxViewer at the following path:  
`Your/Game/Folder/BepInEx/config/rost.moment.unity.hitboxviewer.cfg`  
Open it as a text file using any text editor.

| Config                                | Group         | Type      | Default   | Description |
|---------------------------------------|---------------|-----------|-----------|-------------|
| BoxCollider Color                     | Colors        | `string`  | `#DB220D` | RGBA hex code of the color used to display hitboxes of the BoxCollider type |
| SphereCollider Color                  | Colors        | `string`  | `#0D2FDB` | RGBA hex code of the color used to display hitboxes of the SphereCollider type |
| CapsuleCollider Color                 | Colors        | `string`  | `#28DB0D` | RGBA hex code of the color used to display hitboxes of the CapsuleCollider type |
| MeshCollider Color                    | Colors        | `string`  | `#DBDB0D` | RGBA hex code of the color used to display hitboxes of the MeshCollider type |
| WheelCollider Color                   | Colors        | `string`  | `#DB7B0D` | RGBA hex code of the color used to display hitboxes of the WheelCollider type |
| TerrainCollider Color                 | Colors        | `string`  | `#A020F0` | RGBA hex code of the color used to display hitboxes of the TerrainCollider type |
| BoxCollider2D Color                   | Colors        | `string`  | `#FF19AF` | RGBA hex code of the color used to display hitboxes of the BoxCollider2D type |
| CircleCollider2D Color                | Colors        | `string`  | `#039AFF` | RGBA hex code of the color used to display hitboxes of the CircleCollider2D type |
| CapsuleCollider2D Color               | Colors        | `string`  | `#633310` | RGBA hex code of the color used to display hitboxes of the CapsuleCollider2D type |
| PolygonCollider2D Color               | Colors        | `string`  | `#000000` | RGBA hex code of the color used to display hitboxes of the PolygonCollider2D type |
| EdgeCollider2D Color                  | Colors        | `string`  | `#FFFFFF` | RGBA hex code of the color used to display hitboxes of the EdgeCollider2D type |
| CompositeCollider2D Color             | Colors        | `string`  | `#363636` | RGBA hex code of the color used to display hitboxes of the CompositeCollider2D type |
| NavMeshObstacle Color                 | Colors        | `string`  | `#008080` | RGBA hex code of the color used to display hitboxes of the NavMeshObstacle type |
| Change Collider Visualization Mode    | Key Binds     | `KeyCode` | `F1`      | Key used to change the collider visualization mode |
| Change NavMeshObstacle Visualization Mode | Key Binds | `KeyCode` | `F2`      | Key used to change the NavMeshObstacle visualization mode |
| Update Rate                           | Update        | `int`     | `60`      | Determines how often (in frames) hitbox outlines are recalculated. If 0 or less, hitboxes will not be updated |
| Points Per Radius                     | Visualization | `int`     | `100`     | Number of points used per unit of circle radius; applies to round hitboxes like CircleCollider2D, SphereCollider, etc. |
| Hitbox Line Width                     | Visualization | `float`   | `0.1`     | Line width for hitbox outlines |
| Shader Name                           | Visualization | `string`  | `Unlit/Color` | Shader name used to color hitbox outlines |

Configs can be easily changed using any text editor—just make sure to use the correct data types.


# How to install
For installing you need [BepInEx](https://github.com/BepInEx/BepInEx)

Install it for game, check for «plugins» folder

If it exists, put .dll file from [Releases Page](https://github.com/Rostmoment/HitboxViewer/releases) into it

Done
