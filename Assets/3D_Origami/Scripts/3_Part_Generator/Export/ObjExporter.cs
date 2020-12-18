using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
 
//http://wiki.unity3d.com/index.php/ObjExporter
public class ObjExporter {
 
    public static string MeshToString(MeshFilter mf) {
        Mesh m = mf.sharedMesh;
        Debug.Log(m.name); //clylinderMesh
        Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;
 
        StringBuilder sb = new StringBuilder();
 
        sb.Append("g ").Append(mf.name).Append("\n");
        foreach(Vector3 v in m.vertices) {
            sb.Append($"v {v.x} {v.y} {v.z}\n");
        }
        sb.Append("\n");
        foreach(Vector3 v in m.normals) {
            sb.Append($"vn {v.x} {v.y} {v.z}\n");
        }
        sb.Append("\n");
        foreach(Vector3 v in m.uv) {
            sb.Append($"vt {v.x} {v.y}\n");
        }
        Debug.Log(m.subMeshCount);
        for (int material=0; material < m.subMeshCount; material ++) {
            sb.Append("\n");
            sb.Append("usemtl ").Append(mats[material].name).Append("\n");
            sb.Append("usemap ").Append(mats[material].name).Append("\n");
 
            int[] triangles = m.GetTriangles(material);
            for (int i=0;i<triangles.Length;i+=3) {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", 
                    triangles[i]+1, triangles[i+1]+1, triangles[i+2]+1));
            }
        }
        return sb.ToString();
    }
 
    public static void MeshToFile(MeshFilter mf, string filename) {
        using (StreamWriter sw = new StreamWriter(filename)) 
        {
            sw.Write(MeshToString(mf));
        }
    }
}