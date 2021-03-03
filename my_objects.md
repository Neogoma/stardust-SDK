# Uploading an object

## OBJ Format

OBJ format is the easiest because you only have to upload the file and they will be available for all platforms.

- Go to the [object list page](https://stardust.neogoma.com/object_list)
- Once you're on the object upload page select [create a new object](https://stardust.neogoma.com/add_bundle)
- Select the option "Upload an OBJ file"
- Fill in the name
- Select the "Public object" checkbox if you want to share with other users of the platform
- Upload the OBJ file (max size 10 mb)
- Optional: Upload the MTL files
- Optional: Upload the Texture files (max size 40 mb)
- Click on "Create object"

## Unity asset bundle format

### Video of the process

[![How to create asset bundle](https://img.youtube.com/vi/TRg6cKWQMqI/0.jpg)](https://www.youtube.com/watch?v=TRg6cKWQMqI)

### Platforms
In order to make your object visible you need to build and upload it to different platforms:
- **IOS and Android** are **mandatory** since the SDK is mainly usable on phone (you should use **Unity 2020.2.x**)
- **WebGL** is **recommended** for visualizing your bundle in the editor (you should use **Unity 2020.1.x**)
- **Windows and OSX** are **optional** if you want to see your bundles in the Unity editor

### Interactive objects
The reason we use bundles is that we can embeed our interactive SDK to create **INTERACTIVE** experiences. So you can use the **Hobodream AR Framework** scripts and routines inside your bundles.

#### Hobodream AR SDK
- Import the latest release of the Hobodream AR SDK in your project
- Import the latest release of the Hobodream AR SDK in your bundle project

#### Notes
While you can setup some scripts on your bundles there are some constrained that you should be aware of:
- Scripts references which are not in the Stardust project will be lost (hence the need for a framework)
- Layers aren't exported into the bundle so you will need to synchronize between your bundle project and your Stardust project.
- Some scripts require the same component so test your bundle project carefully before upload.
- If you haev sound and textures, compress them as much as possible to avoid long download times for the other users.
- Shaders and textures can be exported without the need to be on the Stardust project.