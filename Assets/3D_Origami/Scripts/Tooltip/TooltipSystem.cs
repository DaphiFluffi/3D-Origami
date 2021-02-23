using UnityEngine;

//https://www.youtube.com/watch?v=HXFoUGw7eKk
public class TooltipSystem : MonoBehaviour
{
    // Singleton reference 
    private static TooltipSystem current;

    [SerializeField] private Tooltip tooltip = default;
    void Awake()
    {
        current = this;
    }

    public static void Show(string content)
    {
        current.tooltip.SetText(content);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
