# Stardust sdk
This repo is an example on how to use the Stardust SDK for Mapping/Relocation. Feel free to use it as a template for your own projects

# Compatibility
- Unity 2019.4.x or higher
- ARFoundation 4.0.8 (Unity package)
- 


# Prerequesite
The SDK requires you to have a valid Stardust account. You can register a developer account for free at https://stardust.neogoma.com
The SDK relies on the Hobodream framework available at https://github.com/Neogoma/hobodream

# Scenes breakdown
The example is composed of 4 scenes
- **SelectionScene** to select the operation to execute
- **Mapping scene** to create a new map and send the datas of the new map to the Stardust servers
- **Update scene** to update a map with new datas. The map needs to be generated before you can update it.
- **Relocation scene** to relocate into a map.

# How to use the SDK
1. Create a new map and send data to server. 
2. Generate your map, you should receive a mail on the adress provided on your developer account to tell you the status of your map.
3. **(OPTIONAL)**: You can visualize and edit your map on https://stardust.neogoma.com/editor
4. Once your map has been generated, it is available for Navigation

## HOW TO MAP/UPDATE
- Walk at an **easy pace** so there is not brutal changes between two data captures otherwise ARFoundation will loose the phone tracking and your datas may have problem.
- If you turn, **turn slowly** otherwise the relocation at turn positions will have issues.
- The datas that will be used for data generation are the data SENT, if a data is captured but NOT SENT it will not be taken into account during the data generation.
- There is no absolute number of data packages to send but we had good relocation tests with 200 pictures.
- Depending on your datas the map generation can take up to 1 hour.
- If you want to **UPDATE** the map, you need to be at the **SAME POSITION** and **SAME ORIENTATION** before starting the scene.
- You can only update a map if it has already been generated.

## Common components between all scenes
All scenes the mapping/relocate scenes have some components in common:
- **Network provider** which is the main interface to get the networking components
- **Coroutine manager** used by some SDK components to run coroutines
- **API Auto login** this script logs in the user at scene start it's not mandatory but the user **HAS** to be logged in before being able to do any operation
- **Login controller** that manages the login logic
- **Session controller** that manages the different operations on the map sessions dtaas
- **Object controller** that manages the different operations on the user objects
- **Scene loading controller** that manages all the scene loadings

All these components have been regrouped under the **StardustComponents** prefab so whenever you create a new scene you just import it.
If you don't want to use the Login on scene start you can use the **StardustComponentsNoAutoLogin** prefab

## Login
The SDK require the user be logged in the Stardust API. This is why we setup an API autologin in every scene and this is also why all the example script wait for login before launching operations

## Mapper
The SDK has an **AbstractDataUploader** class which is used to send data to the Stardust servers extend this class to have your own export system. In the demo the **Exporter** class serves as a demo implementation.
**NOTE** The ARPointCloud manager is OPTIONAL but be aware that not having it will not upload the point cloud data to the server.
The **ObjectManager** class is an example on how to manage the object creation.
If you need to use standard listener system you don't need to implement **IInteractiveElementListener** again. The methods **HandleSubEvent** and **GetSupportedEventsForSubclass** are substitute to the classic listener methods.


## Updater
Updater is the same structure as the Mapper. The biggest difference is that there is no ARPointCloudManager.


## Navigator
The SDK has an **AbstractMapManager** class which is used to compare phone data with the Stardust servers to get the position. Extend this class for your custom behavior. In the demo the **RelocationDemo** class serves as a demo implementation.
The method ` protected override void OnMapDownloaded(GameObject map)` is called once your map is downloaded and has your map representation in Unity space.
If you need to use standard listener system you don't need to implement **IInteractiveElementListener** again. The methods **HandleSubEvent** and **GetSupportedEventsForSubclass** are substitute to the classic listener methods.


