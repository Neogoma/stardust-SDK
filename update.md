# Update log


## 0.55 
* **SDK**
    * Updating the data capture method

## 0.53
* **SDK**
    * Creating a store account for App store
    * Updating to the latest hobodream package
* **Editor**
    * Upgrade editor to Unity 2020.2.x, you can now create your bundles on Unity 2020
    * Update the datas cache to make the map display faster

## 0.52
* **SDK**
    * Fixed a memory leak by blocking the data capture
    * Making relocation faster by reducing amount of data required

## 0.51
* **SDK**
    * Creating GetDataForMap that uses a map UUID rather than a session object

## 0.5 "Constellation"
* **SDK**
    * Upgrade to Unity 2020.2.x and ARFoundation    
    * Increase data resolution: we increased the volume of data extracted from pictures, this will result in longer upload times and longer relocation times
    * Updated the relocation algorithm: data 
    * Point cloud: extract and send point cloud datas to server
    * Memory management: reduced the memory consuption of the SDK on phone  
    * Send data at any coordinates: allow the **MapDataUploader** to send datas at other origins than (0,0,0), if you want to send data after relocating you can now do it
    * Send object at any coordinates: following previous point, we also allow users to create objects in Unity space or in map space.
    * Cross-platform mapping/relocation: Android phones can relocate on IPhone maps and vice versa
    * Renamed **MatchingPosition** to **RelocationResults**
    * **MapRelocationManager.onPositionFound** now triggers an even with **RelocationResults** and a **CoordinateSystem**
* **Editor**
    * Fast manipulation shortcuts
    * Display the point cloud
    * Update objects and target icons on the mini map
    * Target constraints: if you try to position targets too far from the navigation path, the target won't be saved and 
* **Dashboard**
    * Mail registration confirmation: if you are an already existing user you will need to login into the dashboard to confirm your mail adress

## 0.3 "New year"
* **SDK**
    * OBJ file management: allow developers to download or load __OBJ__     
* **Editor**
    * Asset viewer: we added an asset viewer on the API to allow users to preview the objects online
    * Multiple objects selection: the user can select multiple objects in scene or in the object list
    * Objects transformation on scene: Object can be scaled/rotated/moved directly from the scene
 
## 0.2 "Pathfinder"
* **SDK**
    * Navigations system: adding the **NavigationComponents** prefab and the **PathFindingManager**
    * API Token: replace login only with API token with the **StardustSDK** class setup
* **Editor**
    * Full redesign: update the design and the UX of the editor
    * Navigation targets edition: you can now edit the map to add navigations targets
