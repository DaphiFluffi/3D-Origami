using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://forum.unity.com/threads/combine-children-apply-mesh-collider-to-parent.9001/
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MeshCombine : MonoBehaviour
{
// Start is called before the first frame update
    public void DokuCombine()
    {
        //https://grrava.blogspot.com/2014/08/combine-meshes-in-unity.html
        Matrix4x4 myTransform = transform.worldToLocalMatrix;
        Dictionary<string, List<CombineInstance>> combines = new Dictionary<string, List<CombineInstance>>();
        Dictionary<string , Material> namedMaterials = new Dictionary<string, Material>();
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in meshRenderers)
        {
            foreach (var material in meshRenderer.sharedMaterials)
                if (material != null && !combines.ContainsKey(material.name)) {
                    combines.Add(material.name, new List<CombineInstance>());
                    namedMaterials.Add(material.name, material);
                }
        }

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        foreach(var filter in meshFilters)
        {
            if (filter.sharedMesh == null)
                continue;
            var filterRenderer = filter.GetComponent<Renderer>();
            if (filterRenderer.sharedMaterial == null)
                continue;
            if (filterRenderer.sharedMaterials.Length > 1)
                continue;
            CombineInstance ci = new CombineInstance
            {
                mesh = filter.sharedMesh,
                transform = myTransform*filter.transform.localToWorldMatrix
            };
            combines[filterRenderer.sharedMaterial.name].Add(ci);

            //Destroy(filterRenderer);
        }

        foreach (Material m in namedMaterials.Values)
        {
            var go = new GameObject("Combined mesh");
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;

            var filter = go.AddComponent<MeshFilter>();
            filter.mesh.CombineMeshes(combines[m.name].ToArray(), true, true);

            var arenderer = go.AddComponent<MeshRenderer>();
            arenderer.material = m;
        }
    
    }

    public void Combine()
    {
        Quaternion OldRot = transform.rotation;
        Vector3 OldPos = transform.position;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;



        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

        Debug.Log(name + " is combining " + filters.Length + " meshes!");

        Mesh finalMesh = new Mesh ();
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent< MeshCollider>().sharedMesh= finalMesh;

        CombineInstance[] combiners = new CombineInstance[filters.Length];


        for(int a = 0 ; a < filters.Length ; a++)
        {
            if(filters[a].transform == transform)
                continue;


            combiners[a].subMeshIndex = 0;
            combiners[a].mesh = filters [a].sharedMesh;
            combiners[a].transform = filters[a].transform.localToWorldMatrix;
        }

        finalMesh.CombineMeshes (combiners);


        GetComponent<MeshFilter>().sharedMesh = finalMesh;



        transform.rotation = OldRot;
        transform.position = OldPos;


        for (int a = 0; a < transform.childCount; a++)
        {
            transform.GetChild(a).gameObject.SetActive(false);
        }
    }

}