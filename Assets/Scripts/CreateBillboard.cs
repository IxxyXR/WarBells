using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateBillboard : MonoBehaviour {

/*
Make a billboard out of an object in the scene
The camera will auto-place to get the best view of the object so no need for camera adjustment

To use - place an object in an empty scene with just camera and any lighting you want.
Add this script to your scene camera and link to the object you want to render.
Press play and you will get a snapshot of the object (looking down the +Z-axis at it) saved out to billboard.png in your project folder
Any pixels colored the same as the camera background color will be transparent
*/

 
 public GameObject objectToRender;
 public int imageWidth = 128;
 public int imageHeight = 128;
 //**bool to only conver once
 private bool grab = false;

 void Start()
 {
     CameraSetup();
 }

 void OnPostRender()
 {
     if (!grab) { ConvertToImage(); grab = true; }
 }

 //**split into two methods
 void CameraSetup()
 {
     //grab the main camera and mess with it for rendering the object - make sure orthographic
     Camera cam = Camera.main;
     cam.orthographic = true;

     //render to screen rect area equal to out image size
     float rw = imageWidth;
     rw /= Screen.width;
     float rh = imageHeight;
     rh /= Screen.height;
     cam.rect = new Rect(0, 0, rw, rh);
     //**manually set the background color
     cam.backgroundColor = new Vector4(0,0,0,0);

     //grab size of object to render - place/size camera to fit
     Bounds bb = objectToRender.GetComponent<Renderer>().bounds;

     //place camera looking at centre of object - and backwards down the z-axis from it
     cam.transform.position = bb.center;
     cam.transform.position.Set(cam.transform.position.x, cam.transform.position.y, -1.0f + (bb.min.z * 2.0f));
     //make clip planes fairly optimal and enclose whole mesh
     cam.nearClipPlane = -1f;
     cam.farClipPlane = -cam.transform.position.z + 10.0f + bb.max.z;
     //set camera size to just cover entire mesh
     cam.orthographicSize = 1.01f * Mathf.Max((bb.max.y - bb.min.y) / 2.0f, (bb.max.x - bb.min.x) / 2.0f);
     cam.transform.position.Set(cam.transform.position.x, cam.orthographicSize * 0.05f, cam.transform.position.y);
 }

 void ConvertToImage()
 {
     var tex = new Texture2D(imageWidth, imageHeight);
     // Read screen contents into the texture
     tex.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
     tex.Apply();

     //turn all pixels == background-color to transparent
     Camera cam = Camera.main;
     Color bCol = cam.backgroundColor;
     Color alpha = new Vector4(0,0,0,0);
     alpha.a = 0.0f;
     for (int y = 0; y < imageHeight; y++)
     {
         for (int x = 0; x < imageWidth; x++)
         {
             Color c = tex.GetPixel(x, y);
             //**check for difference
             if (c.r != bCol.r)
                 tex.SetPixel(x, y, new Vector4(c.r, c.g, c.b, 1));
         }
     }
     tex.Apply();
     
     // Encode texture into PNG
     byte[] bytes = tex.EncodeToPNG();
     Destroy(tex);

     //**path is in Assets, you must refresh Unity to see the file (can be done by clicking away then click on Unity window again, etc)
     System.IO.File.WriteAllBytes(Application.dataPath + "/" + objectToRender.name + ".png", bytes);
 }
}
