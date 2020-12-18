using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityFBXExporter;


public class Done : MonoBehaviour
{
    [Tooltip("Click here to export objectToExport as .fbx with materials.")]
    [SerializeField]
    bool exportFBX;
    //[Tooltip("Root object of what will be exported as .fbx file.")]
   // [SerializeField]
    GameObject objectToExport;
    FBXExporter fBXExporter;
    private GameObject objMeshToExport;
    private GameObject cylinder;
    [SerializeField] private Material doubleSided;
    private Mesh finalMesh;
    
    private void Start()
    {
        fBXExporter = new FBXExporter();
    }
    
    /*public void Combine()
    {
        cylinder = GameObject.FindGameObjectWithTag("Cylinder");
        finalMesh = cylinder.GetComponent<MeshCombine>().Combine();
        GameObject go = new GameObject();
        go.AddComponent<MeshCollider>();
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshCombine>();
        go.GetComponent<MeshFilter>().sharedMesh = finalMesh;
        go.GetComponent<MeshRenderer>().material = doubleSided;
       // Instantiate(go);
    }*/
    
    public void Convert()
    {
        //if(exportFBX)
        //{
        exportFBX = false;
        
        cylinder = GameObject.FindGameObjectWithTag("Cylinder");
       // finalMesh = cylinder.GetComponent<MeshCombine>().Combine();
       /* GameObject go = new GameObject();
        go.AddComponent<MeshCollider>();
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshCombine>();
        go.GetComponent<MeshFilter>().sharedMesh = finalMesh;*/
        cylinder.GetComponent<MeshRenderer>().material = doubleSided;
     
        objectToExport = cylinder;
        if(objectToExport == null)
        {
            Debug.LogError($"Please assign an object to export as an .fbx file.", this);
            return;
        }

        string path = EditorUtility.SaveFilePanel($"Export {objectToExport} as .fbx", "", objectToExport.name + ".fbx", "fbx");
        Debug.Log(path);
        FBXExporter.ExportGameObjToFBX(objectToExport, path, true, true);
        //}
    }
    
    //https://stackoverflow.com/questions/46733430/convert-mesh-to-stl-obj-fbx-in-runtime
   /* public void ExportOBJ()
    {
        string path = Path.Combine(Application.persistentDataPath, "data");
       // path = Path.Combine(path, "carmodel" + ".obj");
        path = Path.Combine(path, "carmodel" + ".dae");

        Debug.Log(path);

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
        MeshFilter meshFilter = objMeshToExport.GetComponent<MeshFilter>();
        Mesh myMesh = meshFilter.sharedMesh;
       // ObjExporter.MeshToFile(meshFilter, path);
        
        ColladaExporter export = new ColladaExporter(path, true);
        export.AddGeometry("MyMeshId", myMesh);
        export.AddGeometryToScene("MyMeshId", "MyMeshName");
        export.Save();
    }*/
}
