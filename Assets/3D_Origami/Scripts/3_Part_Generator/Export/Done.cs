using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityFBXExporter;

public class Done : MonoBehaviour
{
    // uses fbx exporter package by https://github.com/KellanHiggins/UnityFBXExporter
    GameObject objectToExport;
    //FBXExporter fBXExporter;
    private GameObject cylinder;
    [SerializeField] private Material doubleSided = default;
    //[DllImport("__Internal")] private static extern void JS_FileSystem_Sync();
	/*void Start()
	{
		fBXExporter = new FBXExporter();
    }*/

    public void Convert()
    {
        cylinder = GameObject.FindGameObjectWithTag("Cylinder");
        cylinder.GetComponent<MeshRenderer>().material = doubleSided;
     
        objectToExport = cylinder;
       /* if(objectToExport == null)
        {
            Debug.LogError($"Please assign an object to export as an .fbx file.", this);
            return;
        }*/

        #if UNITY_EDITOR
            string path = EditorUtility.SaveFilePanel($"Export {objectToExport} as .fbx", "", objectToExport.name + ".fbx", "fbx");
            FBXExporter.ExportGameObjToFBX(objectToExport, path, true, true);
        #elif UNITY_WEBGL
            string content = FBXExporter.MeshToString(objectToExport, null, true, true);
            WebGLFileSaver.SaveFile(content, "cylinder.fbx", "application/octet-stream");
        #elif UNITY_STANDALONE
        #elif UNITY_ANDROID	
        #endif
    }
}
