using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Done : MonoBehaviour
{
    public RawImage image;

    public void TakeScreenshot()
    {
        //https://forum.unity.com/threads/generating-sprites-dynamically-from-png-or-jpeg-files-in-c.343735/
        //ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height); 

        /* var sprite = Resources.Load<Sprite>("/Resources/CameraScreenshot");
         image.GetComponent<SpriteRenderer>().sprite = sprite;*/

        /* TextureImporter importer = AssetImporter.GetAtPath("/CameraScreenshot") as TextureImporter;
         if (importer == null) {
             Debug.LogError("Could not TextureImport from path: " + "/CameraScreenshot");
         }else {
             importer.textureType = TextureImporterType.Sprite;
             importer.spriteImportMode = SpriteImportMode.Single;
             importer.SaveAndReimport();
         }*/
    }
    
}
