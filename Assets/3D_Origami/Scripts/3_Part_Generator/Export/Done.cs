using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    private GameObject objMeshToExport;

    //https://stackoverflow.com/questions/46733430/convert-mesh-to-stl-obj-fbx-in-runtime
    public void ExportOBJ()
    {
        objMeshToExport = GameObject.FindGameObjectWithTag("Cylinder");
        objMeshToExport.AddComponent<MeshCombine>();
        objMeshToExport.GetComponent<MeshCombine>().DokuCombine();
        //only works in Editor, obstructs Build
        
      //  AssetDatabase.CreateAsset(objMeshToExport, "Assets/hyperboloid.asset"); //Serialisierung des Mesh assets
        // AssetDatabase.SaveAssets();
        
        string localPath = "Assets/cylinderrrr.prefab";

        // Make sure the file name is unique, in case an existing Prefab has the same name.
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // Create the new Prefab.
        PrefabUtility.SaveAsPrefabAssetAndConnect(objMeshToExport, localPath, InteractionMode.UserAction);
       /* string path = Path.Combine(Application.persistentDataPath, "data");
        path = Path.Combine(path, "carmodel" + ".obj");
        Debug.Log(path);

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        MeshFilter meshFilter = objMeshToExport.GetComponent<MeshFilter>();
        ObjExporter.MeshToFile(meshFilter, path);*/
    }

}
