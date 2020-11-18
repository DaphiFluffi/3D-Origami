using UnityEngine;
using UnityEngine.Events;

//https://docs.unity3d.com/ScriptReference/Events.UnityEvent_1.html
[System.Serializable]
public class ColorEvent : UnityEvent<string, string>
{
    //Color before and after
}

public class ColorOrigami : MonoBehaviour
{
    //TODO can this be private?
    public ColorEvent OnColorChanged;
    private FlexibleColorPicker fcp;
    private Color currentColor;
    private ColorManager colorManager;
    
    void Awake()
    {
        colorManager = FindObjectOfType<ColorManager>();
        fcp = FindObjectOfType<Canvas>().GetComponentInChildren<FlexibleColorPicker>(true);
    }

    void Start()
    {
        if (OnColorChanged == null)
        {
            OnColorChanged = new ColorEvent();
        }

        OnColorChanged.AddListener(colorManager.HowManyPiecesAreTheSameColor);
        currentColor = GetComponent<Renderer>().material.color;
        
    }
    
    //https://answers.unity.com/questions/856790/click-gameobject-to-change-color.html
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
