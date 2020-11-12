using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowFCP : MonoBehaviour
{
    [SerializeField] private FlexibleColorPicker fcp =default;
    private Color currentColor;
    private Canvas canv;
    private int colorCounter;
    private GameObject palettePanel;
    private Image paintPot;
    private TMP_Text amount;
   
    private bool isOn;
   
    public void ShowColorPicker()
    {
        this.gameObject.SetActive(true);
    }

    public void HideColorPicker()
    {
        this.gameObject.SetActive(false);
    }

   /* void Update()
    {
        Debug.Log(fcp.color);
    }*/

}
