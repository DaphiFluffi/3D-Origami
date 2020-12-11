using System;
using UnityEngine;

public class ColorToggle : MonoBehaviour
{
    private bool showing;

    public bool GetShowing()
    {
        return showing;
    }
    public void ShowColorSettings(bool show)
    {
        if (show)
        {
            showing = true; 
            this.gameObject.SetActive(true);
        }
        else
        {
            showing = false; 
            this.gameObject.SetActive(false);
        }
    }
    
}
