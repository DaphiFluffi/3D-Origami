using UnityEngine;
using UnityEngine.UI;

public class ShowHideUI : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToHide = default;

    private bool hidden = true; 
    
    public void ShowOrHideSlider()
    {
        if (hidden)
        {
            Show();
            hidden = false;
        }
        else
        {
            Hide();
            hidden = true;
        }
    }

    public void Show()
    {
        //show 
        objectToHide.gameObject.SetActive(true);
    }

    public void Hide()
    {
        //hide  
        objectToHide.gameObject.SetActive(false);
    }
}
