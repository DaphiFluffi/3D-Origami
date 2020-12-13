using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//https://www.youtube.com/watch?v=HXFoUGw7eKk
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Multiline]
    [SerializeField] private string content = default; 
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(Delay(2f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(Delay(2f));
        TooltipSystem.Hide();
    }

    IEnumerator Delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        TooltipSystem.Show(content);
    }
}
