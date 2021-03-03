# Specific update instructions

## Update
- You can only update a map if it has already been generated.
- In the update, you can scan places that were not previously mapped in the original mapping.
- You can scan multiple updates before effectively run the update.

## Update after relocation
You can have a full example on how to update your map after relocating on the **UpdateAfterRelocateDemo** class.

### Process
1. First you need to listen to the __onPositionFound__ event from the **MapRelocationManager**
2. When the position on map has been found, update the coordinate system of the **MapDataUploader**
3. That's it! Now when the **MapDataUploader** will capture data, it will send these datas in proper map space

### Example
```cs
public void Awake()
{
    //1. Setup listeners for relocation    
    MapRelocationManager.Instance.onPositionFound.AddListener(PositionFound);
}

private void PositionFound(RelocationResults positionMatched,CoordinateSystem newCoords)
{        
    //2. Update the coordinate system   
    MapDataUploader.Instance.UpdateCoordinateSystem(newCoords);    
}

```
## Data quality
To increase the accuracy of relocation, try to update the map at different times of the day and/or with different lighting conditions.