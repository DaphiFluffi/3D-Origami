using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityFBXExporter;
using SimpleFileBrowser;

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

    void Start()
    {
        FileBrowser.SetDefaultFilter( ".fbx" );
    }

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

       string content = FBXExporter.MeshToString(objectToExport, null, true, true);

        #if UNITY_EDITOR
            string path = EditorUtility.SaveFilePanel($"Export {objectToExport} as .fbx", "", objectToExport.name + ".fbx", "fbx");
            FBXExporter.ExportGameObjToFBX(objectToExport, path, true, true);
        #elif UNITY_WEBGL
            WebGLFileSaver.SaveFile(content, "cylinder.fbx", "application/octet-stream");
        #elif UNITY_STANDALONE || UNITY_ANDROID	
        // https://forum.unity.com/threads/simple-file-browser-open-source.441908/page-7#post-6642685
           FileBrowser.ShowSaveDialog( ( paths ) =>
           {
               string targetPath = paths[0];
               FileBrowserHelpers.WriteTextToFile(targetPath, content);
           }, null, FileBrowser.PickMode.Files, false, "C:\\", "object.fbx", "Save As", "Save" );
        #endif
    }
}
