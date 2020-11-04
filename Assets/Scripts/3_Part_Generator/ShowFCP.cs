using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFCP : MonoBehaviour
{
   /* private FlexibleColorPicker fcp;

    void Awake()
    {
        fcp = FindObjectOfType<FlexibleColorPicker>();
        fcp.gameObject.SetActive(false);

    }*/

    [SerializeField] private Button backBtn = default;
    private bool isOn;
   
    public void ShowColorPicker()
    {
        this.gameObject.SetActive(true);
        backBtn.gameObject.SetActive(true);
    }

    public void HideColorPicker()
    {
        this.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(false);
    }

}
