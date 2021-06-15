# Role of the class
The job of the map relocation manager is torun the relocation requests as well as retrieving all the map datas from the server
The Map relocation manager is a singleton that can be called via 
```cs
MapRelocationManager objectController = MapRelocationManager.Instance;
```

## Events
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
## Setup the map first
Before calling the relocation you will need to setup the session you want to relocate in using the __GetDataForMap__ function. It will download the map data and setup the relocation manager on this session. You can use a Session instance or a UUID.

```cs
//Get the map datas using the Session instance
public void SelectSession(Session session){
    MapRelocationManager.Instance.GetDataForMap(selectedSession);                
}

//Get the map datas using the Session UUID (from the dashboard)
public void SelectSession(string uuid){
    MapRelocationManager.Instance.GetDataForMap(uuid);                
}

```

When the relocation is done it will trigger the  __mapDownloadedSucessfully__ event. The event will contain 2 datas:
- The session that was requested
- The gameobject that contains the map data

## Run relocation
In order to run the relocation you just have to do the following call
```cs
MapRelocationManager.Instance.LocateCurrentPosition();
```

If the position can be found then the __positionFound__ event will be triggered otherwise __positionNotFound__ will be triggered.