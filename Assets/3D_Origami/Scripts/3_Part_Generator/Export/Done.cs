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
    private GameObject cylinder;
    [SerializeField] private Material doubleSided = default;
    
    void Start()
    {
        FileBrowser.SetFilters( true, new FileBrowser.Filter( ".fbx", ".fbx" ));
        FileBrowser.SetDefaultFilter( ".fbx" );
    }

    public void Convert()
    {
        cylinder = GameObject.FindGameObjectWithTag("Cylinder");
        cylinder.GetComponent<MeshRenderer>().material = doubleSided;
     
        objectToExport = cylinder;
        
        #if UNITY_EDITOR
            string path = EditorUtility.SaveFilePanel($"Export {objectToExport} as .fbx", "",
                objectToExport.name.ToString() + ".fbx", "fbx");
            FBXExporter.ExportGameObjToFBX(objectToExport, path, true, true);
        #elif UNITY_WEBGL
            string content = FBXExporter.MeshToString(objectToExport, null, true, true);
            WebGLFileSaver.SaveFile(content, "cylinder.fbx", "application/octet-stream");
        #elif UNITY_STANDALONE || UNITY_ANDROID	
        // https://forum.unity.com/threads/simple-file-browser-open-source.441908/page-7#post-6642685
           FileBrowser.ShowSaveDialog( ( paths ) =>
           {
               string content = FBXExporter.MeshToString(objectToExport, null, true, true);
               string targetPath = paths[0];
               FileBrowserHelpers.WriteTextToFile(targetPath, content);
           }, null, FileBrowser.PickMode.Files, false, "C:\\", "3D_Origami_Model_", "Save As", "Save" );
        #endif
    }
    
}
