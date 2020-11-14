using UnityEngine;
using UnityEngine.UI;

public class AccessColorAgain : MonoBehaviour
{
    private FlexibleColorPicker fcp;
    void Start()
    {
        fcp = FindObjectOfType<Canvas>().GetComponentInChildren<FlexibleColorPicker>(true);
    }
    
    public void AccessColor(Image paintPotImage)
    {
        fcp.color = paintPotImage.color;
    }
}
