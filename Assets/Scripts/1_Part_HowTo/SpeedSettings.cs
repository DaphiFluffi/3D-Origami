using UnityEngine;
using UnityEngine.UI;

public class SpeedSettings : MonoBehaviour
{
    [SerializeField]
    private Slider speedSettingsSlider = default;

    private bool hidden = true; 
    
    public void ShowOrHideSlider()
    {
        if (hidden)
        {
            //show slider 
            speedSettingsSlider.gameObject.SetActive(true);
            hidden = false;
        }
        else
        {
            //hide slider 
            speedSettingsSlider.gameObject.SetActive(false);
            hidden = true;
        }
    }
}
