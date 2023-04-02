# Easy Icon Maker
##### Easy Icon Maker is a simple but flexible tool for creating images from 3D objects in Unity Editor.
![Main](https://github.com/Servalstar/Easy-Icon-Maker/blob/master/Screenshots/main_screen.jpg?raw=true)
## Key features:
- creating an image from objects with the MeshRenderer component in a specially created scene
- work from the Unit Editor or from your code
- setting the position, rotation and scale of the object
- setting the direction and color of two light sources
- fill the background with solid color, set a transparent background, or set the image as the background
- the ability to select a folder to save the image (by default, images are saved in the "Assets/Easy Icon Maker" folder)
## Examples of getting an image from the code:
Creating an image will consist of several basic steps:
- create a preview scene
- adding an object with the MeshRenderer component to the scene
- calling the image-making method
- clear scene preview

Optionally, you can customize the scene using the following properties of the scene object:
- GameObject CameraContainer (used for rotation the camera around the object)
- Camera PreviewCamera
- Light Light1
- Light Light2

Texture2D MakeImage(string folderPathf, string name, int size, Texture texBG) method parameters:
- folderPathf - path to save the image. If you specify null, then image will be no saving, if you specify "default", then there will be a save to the folder "Assets/Easy Icon Maker/".
- name - name of the saved file. If you specify "default", then the name will be taken from GameObject.name.
- size - width and height of the square image.
- texBG - image to be used as background.

```csharp
using EasyIconMaker;
//...

#if UNITY_EDITOR

	// creating Preview Scene
	PreviewScene preview = new PreviewScene();
	
	// adding a prefab object to the scene
	preview.InitInstance(go);
	
	
	// getting a texture with a white background and a size of 256x256 pixels without saving the file to disk
	Texture2D tex = preview.MakeImage(null, null, 256, null);  
	
	// getting a texture with a yellow background, size 378x378 pixels and saving it in the "Assets/My folder/" folder with the name icon1.png
	string folderPathf = "Assets/My folder/";
	string name = "icon1";
	preview.PreviewCamera.backgroundColor = Color.yellow;
	Texture2D tex = preview.MakeImage(folderPathf, name, 378, null); 

	// getting a texture with a transparent background, size 512x512 pixels and saving it in the "Assets/Easy Icon Maker/" folder with the GameObject name .png
	preview.PreviewCamera.clearFlags = CameraClearFlags.Nothing;
	Texture2D tex = preview.MakeImage("default", "default", 512, null);  
	
	// creating a texture with a background from your image, size 256x256 and saving it in the "Assets/Easy Icon Maker/" folder with name car.png
	string name = "car";
	Texture myBackground = Resources.Load<Texture>("Textures/backgroundForIcon"));
	preview.PreviewCamera.clearFlags = CameraClearFlags.Nothing;
	preview.MakeImage("default", name, 256, myBackground);
	
	
	// clearing the scene
	preview.Cleanup();
	
#endif

//...
```
