using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorScript : MonoBehaviour
{
    [SerializeField]
    private GameObject orig;
    
    void Start()
    {
        //ok, this is a vible option to instantiate my pieces 
        /*Instantiate(orig, new Vector3(0, 1, 0), Quaternion.identity);
        Instantiate(orig, new Vector3(1, 1, 0), Quaternion.identity);
        Instantiate(orig, new Vector3(0.5f , 3, 0), Quaternion.identity);*/
        Instantiate(orig, new Vector3(0, 1, 0), Quaternion.identity);
        Instantiate(orig, new Vector3(1, 1, 0), Quaternion.identity);
        Instantiate(orig, new Vector3(0.5f , 3, 0), Quaternion.identity);
    }

}
