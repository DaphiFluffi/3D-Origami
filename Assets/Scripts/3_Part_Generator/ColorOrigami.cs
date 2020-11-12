using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class ColorOrigami : MonoBehaviour
{
    private FlexibleColorPicker fcp;
    private Color currentColor;
    private Canvas canv;
    private int colorCounter;
    private GameObject palettePanel;
    private Image paintPot;
    private TMP_Text amount;
    
    void Awake()
    {
        canv = FindObjectOfType<Canvas>();
        fcp = canv.GetComponentInChildren<FlexibleColorPicker>(true);
        palettePanel = GameObject.FindGameObjectWithTag("Palette");
        paintPot = palettePanel.transform.GetChild(0).GetComponent<Image>();
    }

    void Start()
    {
        currentColor = GetComponent<Renderer>().material.color;
    }
    
    //TODO Quelle
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (fcp != null && currentColor != fcp.color)
                { 
                    GetComponent<Renderer>().material.color= fcp.color;
                    Debug.Log("colorChanged");
                    currentColor = fcp.color;
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
