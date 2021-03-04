# Stardust basic components
The SDK has multiple controllers each one managing a different part. We divided into multiple components so each component has its own role. 
- **Session controller** manages all the sessions management (creation/list of maps).
- **Object controller** takes control of all the object management (creation/list of available objects).
- **Map Data Uploader** is in charge of all the data uploading to server.
- **Map Relocation Manager** will be used during relocation, it will also manage the recreation of the map during the relocation part.

## Session controller
The session controller is a singleton that can be called via:
```cs
SessionController sessionController=SessionController.Instance();
```
### Create a new session
To create a new session you need to call the __CreateMappingSession()__ method AFTER setting up a listener method on the __onSessionCreationSuccess__ event.

The name will be generated randomly but you can modify it in the dashboard.
```cs
public void Start(){

    //Get the session controller instance
    SessionController sessionController=SessionController.Instance();

    //Setup a listener to be called when the session creation will be sucessfull
    sessionController.onSessionCreationSucess.AddListener(SessionCreated);

    //Create a new session
    sessionController.CreateMappingSession();
}

//Will be called when the new session will be created
private void SessionCreated(Session session)
{
    Debug.Log(session.name);
}
```

### Get the list of session
The session manager also has a method to retrieve the list of **READY** sessions (ready means the session can be relocated).
```cs
public void Start(){
    //Get the session controller instance
    SessionController sessionController=SessionController.Instance();

    //Setup a listener to be called when the map download list is done
    sessionController.onAllSessionsRetrieved.AddListener(MapListDownloaded);

    //Get all the sessions ready now
    sessionController.GetAllSessionsReady();

}   

//Will be called when the list of map will be downloaded
private void MapListDownloaded(Session[] allSessions)
{
    for (int i = 0; i < allSessions.Length; i++)
    {
        Debug.Log(allSession[i].name);
    }
}
```

## Map data uploader
The Map data uploader is a singleton that can be called via 
```cs
MapDataUploader dataUploader = MapDataUploader.Instance;
```

### Events
The map data uploader has multiple events depending on the situation
```cs
public void Start(){
    //Get instance
    MapDataUploader dataUploader = MapDataUploader.Instance;
    
    //This event when data will be captured on device
    dataUploader.dataCapturedSucessfully.AddListener(OnDataCaptured);
    
    //This event will be called when data was successfully sent to server
    dataUploader.dataSentSucessfully.AddListener(OnDataSentSuccess);
    
    //This event will be called if you reach your data limit
    dataUploader.datalimitReached.AddListener(OnDataLimitReached);
}


private void OnDataCaptured(int datacount)
{
    Debug.Log("Data captured" + datacount);
}

private void OnDataSentSuccess(int uploadCount)
{
    Debug.Log("Data captured" + uploadCount);
}

private void OnDataLimitReached()
{
    Debug.Log("Maximum pictures for map reached");
}
```

### Start/Stop uploading datas
You can easily control when you send datas or when you stop sending datas via the methods provided by the singleton.

Please note that if you stop the capture, the datas that are stored on device but not sent will still be sent to server.
```cs
//Start sending data to server
public void StartCapturingData(){
    MapDataUploader.Instance.StartSendingData();
}

//Stops sending data to server
public void StopCapturingData(){
    MapDataUploader.Instance.StopSendingDatas();
}
```

### Run generation (new map)
If you run a generation, be aware that the generation will be done on **UPLOADED DATAS ONLY**.

```cs
public void GenerateMap(){
    MapDataUploader.Instance.GenerateMap();
}
```

### Run update (existing map)
In order to update a map you need to setup the session first
```cs
public void SetActiveSession(Session session){
    MapDataUploader.Instance.SetSession(selectedSession);
}
```

Once the active session has been setup you can start uploading datas. Once the needed datas have been uploaded you can call the update with the following call

```cs
public void UpdateMap(){
    MapDataUploader.Instance.UpdateMap();
}
```

## Map relocation manager
The Map relocation manager is a singleton that can be called via 
```cs
MapRelocationManager objectController = MapRelocationManager.Instance;
```

### Events
The Map relocation manager handles the download of the map data as well as the relocation requests.

```cs
public void Start{
    //Get instance
    MapRelocationManager relocationManager = MapRelocationManager.Instance;

    //Called when the map has been sucessfully downloaded
    relocationManager.onMapDownloadedSucessfully.AddListener(OnMapDownloaded);

    //Called when the map starts downloading
    relocationManager.onMapDownloadStarted.AddListener(OnMapStartDownloading);

    //Called when the position in map has been found after relocation request
    relocationManager.onPositionFound.AddListener(OnPositionMatched);

    //Called when the position in map has not been found after relocation request
    relocationManager.onPositionNotFound.AddListener(OnPositionMatchFailed);
}

private void OnMapDownloaded(Session session,GameObject map)
{
    Debug.Log("Map downloaded");    
}

private void OnMapStartDownloading()
{
    Debug.Log("Start to download the map");
}

private void OnPositionMatched(RelocationResults positionMatched,CoordinateSystem newCoords)
{
    Debug.Log("You are at " + positionMatched.LocatedPosition);  
}

private void OnPositionMatchFailed()
{
    Debug.Log("Could not find position");
}
```
### Setup the map first
Before calling the relocation you will need to setup the session you want to relocate in using the __GetDataForMap__ function. It will download the map data and setup the relocation manager on this session.

```cs
public void SelectSession(Session session){
    MapRelocationManager.Instance.GetDataForMap(selectedSession);                
}
```

When the relocation is done it will trigger the  __mapDownloadedSucessfully__ event. The event will contain 2 datas:
- The session that was requested
- The gameobject that contains the map data

### Run relocation
In order to run the relocation you just have to do the following call
```cs
MapRelocationManager.Instance.LocateCurrentPosition();
```

If the position can be found then the __positionFound__ event will be triggered otherwise __positionNotFound__ will be triggered.

## Object controller
The Object controller is a singleton that can be called via 
```cs
ObjectController objectController = ObjectController.Instance;
```

### Get the list of available objects
The object manager has a method to get your objects as well as every public objects.
```cs
public void Start(){
    //Get the instance of the object controller
    ObjectController objectController = ObjectController.Instance;

    //Add a listener when the list has been retrieved
    objectController.objectListDownloaded.AddListener(ObjectListDownloaded);

    //Request the object list
    objectController.RequestAllObjects();
}

//Called when the list of object has been downloaded
private void ObjectListDownloaded(){
    List<Bundle> objectList = objectController.GetAllAvailableObjects();

    for (int i = 0; i < objectList.Length; i++){
        Debug.Log(objectList[i].name);
    }
}
```

### Instanciate a new object
#### Setup a prefab
First you need to setup the prefab in the Object Controller. A simple example prefab is provided in the SDK, called **BundleHolder**. This example displays a loading object to notify the user that the object is currently downloading.

You can create your own prefab with your own implementation of **AbstractBundleDisplayer** or reuse **BundleDisplayerExample**.
#### Write the code
This operation requires a bundle so make sure you know exactly which bundle you want to create, you can use the method above to know the list of bundles you can select from.
```cs
//Session has to be setup from outside
private Session session;

//
private ObjectController objectController;
private Bundle selectedBundle;


public void Start(){
    //Get the instance of the object controller
    objectController = ObjectController.Instance;

    //Add a listener when the list has been retrieved
    objectController.objectListDownloaded.AddListener(ObjectListDownloaded);

    //Request the object list
    objectController.RequestAllObjects();
}

//Called when the list of object has been downloaded
private void ObjectListDownloaded(){
    List<Bundle> objectList = objectController.GetAllAvailableObjects();
    
    //Selecting the first bundle for creation
    selectedBundle=objectList[0];
}

//Call this method to create an object
public void CreateSelectedObject()
{
    //Create object at camera position
    Vector3 pos = Camera.Main.position;
    Quaternion rot = Quaternion.identity;    
    Vector3 scale=Vector3.one;

    //Instantiate object and save it on server
    objectController.CreateAndSaveObject(pos, rot, session, scale,selectedBundle);    
    
}

```
