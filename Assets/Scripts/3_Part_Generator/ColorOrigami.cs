using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOrigami : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                this.GetComponent<Renderer>().material.color = Color.red;
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
