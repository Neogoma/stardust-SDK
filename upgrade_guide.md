# Stardust upgrade guide

## Upgrade from 0.1x to 0.2

### Before importing the package
- Delete the **hobodream-sdk** from your project

### After importing the package
- You can delete the old **StardustComponents** and **StardustComponentsNoAutoLogin** from your project.
- Import the new **StardustComponents** from the new SDK.
- Get your developer api key from the [dashboard](https://stardust.neogoma.com/profile).
- For better naming conventions **ALL** Uunity events have been renamed with the "on" prefix for example __dataCapturedSucessfully__ has been renamed to __onDataCapturedSucessfully__. Just rename all your event refrences with the prefix "on"