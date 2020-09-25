# Stardust sdk
This repo is an example on how to use the Stardust SDK for Mapping/Relocation. Feel free to use it as a template for your own projects

# Compatibility
- Unity 2019.4.x or higher
- ARFoundation 4.0.8 (Unity package)
- AR subsystems 4.0.8 (Unity package)
- Unity ARKit plugin 4.0.8 (Unity package)
- Unity ARCore XR plugin 4.0.8 (Unity package)


# Issues
In case of problems, please submit a [new issue](https://github.com/Neogoma/stardust/issues) after checking that your issue was not already reported.
Issues not following template will not be prioritized compared to templates issue.


# Prerequesite
The SDK requires you to have a **valid Stardust** account. You can apply for a developer account for free [here](https://pcdev1.neogoma.com/register#/).
Once your developer account is active you can use the SDK.

The SDK relies on the [Hobodream framework](https://github.com/Neogoma/hobodream)

# Scenes breakdown
The example is composed of 4 scenes
- **SelectionScene** to select the operation to execute
- **Mapping scene** to create a new map and send the datas of the new map to the Stardust servers
- **Update scene** to update a map with new datas. The map needs to be generated before you can update it.
- **Relocation scene** to relocate into a map.

# How to use the SDK
1. Create a new map and send data to server. 
2. Generate your map, you should receive a mail on the adress provided on your developer account to tell you the status of your map.
3. **(OPTIONAL)**: You can visualize and edit your map on the [editor](https://stardust.neogoma.com/editor)
4. Once your map has been generated, it is available for Navigation

## Mapping update instructions
Please refer to the [wiki page](https://github.com/Neogoma/stardust/wiki/Creating-and-updating-a-map-instructions) for mapping.

## Common components between all scenes
All scenes the mapping/relocate scenes have some components in common:
- **Network provider** which is the main interface to get the networking components
- **Coroutine manager** used by some SDK components to run coroutines
- **API Auto login** this script logs in the user at scene start it's not mandatory but the user **HAS** to be logged in before being able to do any operation. Once the user is logged in once, it's not necessary to call login again.
- **Login controller** that manages the login logic
- **Session controller** that manages the different operations on the map sessions dtaas
- **Object controller** that manages the different operations on the user objects
- **Scene loading controller** that manages all the scene loadings

All these components have been regrouped under the **StardustComponents** prefab so whenever you create a new scene you just import it.
If you don't want to use the Login on scene start you can use the **StardustComponentsNoAutoLogin** prefab

## Login
The SDK require the user be logged in the Stardust API. This is why we setup an **API autologin** in the first scene. Fill in the **API Autologin** script with your informations. 


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


## Request Limitation
Free users are allowed 10 maps and 400 pictures per map.

## Upload your own objects
Refer to the [wiki page](