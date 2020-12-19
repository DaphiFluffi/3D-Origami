using UnityEngine;
using UnityEditor;
using UnityFBXExporter;


public class Done : MonoBehaviour
{
    // uses fbx exporter package by https://github.com/KellanHiggins/UnityFBXExporter
    GameObject objectToExport;
    FBXExporter fBXExporter;
    private GameObject cylinder;
    [SerializeField] private Material doubleSided = default;

    private void Start()
    {
        fBXExporter = new FBXExporter();
    }
    
    public void Convert()
    {
        cylinder = GameObject.FindGameObjectWithTag("Cylinder");
        cylinder.GetComponent<MeshRenderer>().material = doubleSided;
     
        objectToExport = cylinder;
        if(objectToExport == null)
        {
            Debug.LogError($"Please assign an object to export as an .fbx file.", this);
            return;
        }

        string path = EditorUtility.SaveFilePanel($"Export {objectToExport} as .fbx", "", objectToExport.name + ".fbx", "fbx");
       // Debug.Log(path);
        FBXExporter.ExportGameObjToFBX(objectToExport, path, true, true);
    }
}
