# Role of the class
This class is in charge of controlling the informations for sessions.

The session controller is a singleton that can be called via:
```cs
SessionController sessionController=SessionController.Instance();
```
## Create a new map
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

## Get the list of session
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
