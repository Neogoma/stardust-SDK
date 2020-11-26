# Uploading an object

## Video of the process

[![How to create asset bundle](https://img.youtube.com/vi/TRg6cKWQMqI/0.jpg)](https://www.youtube.com/watch?v=TRg6cKWQMqI)

## Platforms
In order to make your object visible you need to build and upload it to different platforms:
- **IOS and Android** are **mandatory** since the SDK is mainly usable on phoe
- **WebGL** is **recommended** for visualizing your bundle in the editor
- **Windows and OSX** are **optional** if you want to see your bundles in the Unity editor

## Interactive objects
The reason we use bundles is that we can embeed our interactive SDK to create **INTERACTIVE** experiences. So you can use the **Hobodream AR Framework** scripts and routines inside your bundles.

### Hobodream AR SDK
- Import the latest release of the Hobodream AR SDK in your project
- Import the latest release of the Hobodream AR SDK in your bundle project

### Notes
While you can setup some scripts on your bundles there are some constrained that you should be aware of:
- Scripts references which are not in the Stardust project will be lost (hence the need for a framework)
- Layers aren't exported into the bundle so you will need to synchronize between your bundle project and your Stardust project.
- Some scripts require the same component so test your bundle project carefully before upload.
- If you haev sound and textures, compress them as much as possible to avoid long download times for the other users.
- Shaders and textures can be exported without the need to be on the Stardust project.
