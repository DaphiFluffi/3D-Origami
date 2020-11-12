using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowFCP : MonoBehaviour
{
    
    private bool isOn;
   
    public void ShowColorPicker()
    {
        this.gameObject.SetActive(true);
    }

    public void HideColorPicker()
    {
        this.gameObject.SetActive(false);
    }
    
}
