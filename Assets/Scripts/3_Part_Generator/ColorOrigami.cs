using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOrigami : MonoBehaviour
{
    private FlexibleColorPicker fcp;

    void Awake()
    {
         fcp = Resources.FindObjectsOfTypeAll<FlexibleColorPicker>()[0]; //find the colorpicker Object even though it is disabled, returns list -> [0]
    }
    
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (fcp != null)
                {
                    this.GetComponent<Renderer>().material.color = fcp.color;
                }
            }
        }
    }

    

    /*void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                currentColor = (currentColor+1)%length;
                this.GetComponent<MeshRenderer>().material.color = colors [currentColor];
            }
        }
    }*/
}
