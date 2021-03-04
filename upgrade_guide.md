# Stardust upgrade guide

## Upgrade from 0.2/0.3 to 0.5

### MapRelocationManager major change
- Update to Unity 2020.2.x
- Update to ARFoundation 4.1.5 (Unity package)
- Update to AR subsystems 4.1.5 (Unity package)
- Update to Unity ARKit plugin 4.1.5 (Unity package)
- Update to Unity ARCore XR plugin 4.1.5 (Unity package)
- **MatchingPosition** has been renamed to **RelocationResults**
- **MapRelocationManager** __onPositionFound__ event now returns 2 objects: **RelocationResults** and **CoordinateSystem**

## Upgrade from 0.1x to 0.2

### Before importing the package
- Delete the **hobodream-sdk** from your project

### After importing the package
- You can delete the old **StardustComponents** and **StardustComponentsNoAutoLogin** from your project.
- Import the prefab **StardustComponents** from the new SDK in your scene.
- Import the prefab **HobodreamComponents** from the new SDK in your scene.
- Get your developer api key from the [dashboard](https://stardust.neogoma.com/profile).
- Fill the API key into the __Stardust SDK__ script of the **StardustComponents** prefab.
- For better naming conventions **ALL** Uunity events have been renamed with the "on" prefix for example __dataCapturedSucessfully__ has been renamed to __onDataCapturedSucessfully__. Just rename all your event refrences with the prefix "on"
