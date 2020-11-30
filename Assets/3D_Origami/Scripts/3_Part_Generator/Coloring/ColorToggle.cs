using UnityEngine;

public class ColorToggle : MonoBehaviour
{
    public void ShowColorSettings(bool show)
    {
        if (show)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
    
}
