using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using UnityEngine.Events;

//https://docs.unity3d.com/ScriptReference/Events.UnityEvent_1.html
[System.Serializable]
public class ColorEvent : UnityEvent<Color>
{
}

public class ColorOrigami : MonoBehaviour
{
    public ColorEvent ColorChanged;
    private FlexibleColorPicker fcp;
    private Color currentColor;
    private Canvas canv;
    private int colorCounter;
    private GameObject palettePanel;
    private Image paintPot;
    private TMP_Text amount;
    private ColorManager manager;
    void Awake()
    {
        manager = FindObjectOfType<ColorManager>();
        canv = FindObjectOfType<Canvas>();
        fcp = canv.GetComponentInChildren<FlexibleColorPicker>(true);
        palettePanel = GameObject.FindGameObjectWithTag("Palette");
        paintPot = palettePanel.transform.GetChild(0).GetComponent<Image>();
        
    }

    void Start()
    {
        if (ColorChanged == null)
            ColorChanged = new ColorEvent();

        ColorChanged.AddListener(manager.Ping);
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
                    //Debug.Log("colorChanged");
                    currentColor = fcp.color;
                    colorCounter++;
                    // TODO Send out "colorChanged" event to ColorManager ListenerScript
                    //Debug.Log(colorCounter);
                    ColorChanged.Invoke(fcp.color);
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
