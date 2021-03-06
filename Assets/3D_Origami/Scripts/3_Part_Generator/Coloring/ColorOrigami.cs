﻿using UnityEngine;
using UnityEngine.Events;

//https://docs.unity3d.com/ScriptReference/Events.UnityEvent_1.html
[System.Serializable]
public class ColorEvent : UnityEvent<string, string>
{
    //Color Hex before and after
}

public class ColorOrigami : MonoBehaviour
{
    private ColorEvent OnColorChanged;
    private FlexibleColorPicker fcp;
    private Color currentColor;
    private ColorManager colorManager;
    
    void Start()
    {
        colorManager = FindObjectOfType<ColorManager>();
        fcp = FindObjectOfType<Canvas>().GetComponentInChildren<FlexibleColorPicker>(true);
        if (OnColorChanged == null)
        {
            OnColorChanged = new ColorEvent();
        }

        OnColorChanged.AddListener(colorManager.HowManyPiecesAreTheSameColor);
        currentColor = GetComponent<Renderer>().material.color;
    }
    
    //http://answers.unity.com/answers/857575/view.html
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
                    OnColorChanged.Invoke(ColorUtility.ToHtmlStringRGB(GetComponent<Renderer>().material.color), ColorUtility.ToHtmlStringRGB(fcp.color));
                    GetComponent<Renderer>().material.color= fcp.color;
                    currentColor = fcp.color;
                }
            }
        }
    }
}
