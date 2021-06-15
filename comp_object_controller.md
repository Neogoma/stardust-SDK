# Object controller
The object controller is in charge of everything concerning the object datas.
The Object controller is a singleton that can be called via 
```cs
ObjectController objectController = ObjectController.Instance;
```

## Get the list of available objects
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

## Instanciate a new object
### Setup a prefab
First you need to setup the prefab in the Object Controller. A simple example prefab is provided in the SDK, called **BundleHolder**. This example displays a loading object to notify the user that the object is currently downloading.

You can create your own prefab with your own implementation of **AbstractBundleDisplayer** or reuse **BundleDisplayerExample**.
### Write the code
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
