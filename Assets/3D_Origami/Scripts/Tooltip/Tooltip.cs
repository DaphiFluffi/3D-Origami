using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//https://www.youtube.com/watch?v=HXFoUGw7eKk
public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentField = default;

    [SerializeField] private LayoutElement layoutElement = default;

    [SerializeField] private int characterWrapLimit = default;

    [SerializeField] private RectTransform rectTransform = default;

    [SerializeField] private Image img = default;

    void Awake()
    {
        this.gameObject.SetActive(false);
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string content)
    {
        contentField.text = content;

        int contentLength = contentField.text.Length;

        if (contentLength > characterWrapLimit)
        {
            layoutElement.enabled = true;
        }
        else
        {
            layoutElement.enabled = false;
        }
    }

    void Update()
    {
        if (this.gameObject == enabled)
        {
            StartCoroutine(FadeImageIn());
        }
        else
        {
            StopAllCoroutines();
        }

        Vector2 position = Input.mousePosition;
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }

    IEnumerator FadeImageIn()
    {
        // loop over 1 second
        for (float i = 0; i < 1.01f; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }
}
