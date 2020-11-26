# Stardust navigation components
Since version 0.2 the Stardust SDK includes a navigation component.

## Principle
The mapping system automatically generates a navigable area based on the datas you sent. So the more you map/update, the better your navigation will be.
Navigations targets are destinations that the user can navigate to in your map. For now the navigation targets can only be defined in the online editor.
The editor will display the navigable area so you can easily see what areas you can go to and what areas you should update. 

**Note** You can setup your target while the map is training.

## Unity SDK
### NavigationComponents prefab
The Navigator component prefab is separated from the other prefab because not all use cases for stardust require navigation. If you do not need navigation in your relocation scene, you don't need to add it. What you will need however is a prefab for drawing the path.

![Navigation setup](_img/pathfinding_setup.jpg)

### Pathfinding manager
The pathfinding manager is the main component responsible for calculating paths to destination. As every basic component it's a singleton that you can get with the following command.
```
PathFindingManager  pathfindingManager = PathFindingManager.Instance;
```

### Events

```
public void Start(){
    PathFindingManager  pathfindingManager = PathFindingManager.Instance;

    //Triggered when the navigation data download starts
    pathfindingManager.startingDownload.AddListener(DownloadStarted);
    
    //Triggered when the navigation data downlad is finished
    pathfindingManager.navigationReady.AddListener(PathFindingReady);
 
    //Triggered when the system starts calculating pathfinding
    pathfindingManager.pathCalculated.AddListener(FinishedPathFinding);

    //Triggered when the system had finished calculating pathfinding
    pathfindingManager.pathCalculated.AddListener(PathFindingReady);
}

//Called when targets have been retrieved
private void PathFindingReady(List<ITarget> allTargets)
{
    //Display the name of every target
    for(int i=0;i<allTargets.Count;i++){
        Debug.Log(allTargets[i].name);
    }
}

private void StartPathfinding(){
    Debug.Log("Pathfinding calculation started");
}

private void FinishedPathfinding(){
    Debug.Log("Pathfinding calculation finished);
}
```

### Display path to target
Once you selected a target you can just show the path to the target using the following code.
```
pathfindingManager.ShowPathToTarget(target);
```

This will automatically find the shortest path from your position to the designated target. Again this only works in navigable areas defined in mapping/update.

## Editor
You can open any map on the editor and select the navigation icon to switch to navigation mode

![Navigation icon](_img/navigation_icon.png)

Once the navigation icon is selected you will see the navigable area as a green overlay.

![Overlay](_img/editor_path.jpg)

### Note
You can put a target anywhere you want but be aware that if you do not put your target on an navigable area the pathfinding algorithm can't guarantee the correct result of your navigation. If you want to make an area navigable just your map.

