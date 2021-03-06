using UnityEngine;
using TMPro;
public class CalculateWidthHeight : MonoBehaviour
{
    private float width;
    private float height;
    [SerializeField] private TMP_Text widthTMP = default;
    [SerializeField] private TMP_Text heightTMP = default;
    
    public void CalculateDimensions(int topRowIndex, int amountPerRow)
    {
        width = 3.4f + (0.05f * amountPerRow);
        height = 2f + 0.5f * (topRowIndex);
        widthTMP.text = "width: " + width + " cm";
        heightTMP.text = "height: " + height + " cm";
    }
    
    public void CalculateDimensions(bool addOrRemove)
    {
        //true = add
        if (addOrRemove)
        {
            height += 0.5f;
        }
        else
        {
            height -= 0.5f;
        }
        heightTMP.text = "height: " + height + " cm";
    }

    public void CalculateDimensions(bool[] invertedInfo)
    {
        // inverting rows
        if (invertedInfo[2])
        {
            // if we have an inverted row in the first row 
            if (invertedInfo[0])
            {
                height -= 0.5f;
            }

            // if there are inverted rows at the top 
            if (invertedInfo[1])
            {
                height += 0.5f;
            }
        }
        // reverting rows
        else
        {
            // revert first row
            if (!invertedInfo[0])
            {
                height += 0.5f;
            }

            // revert inverted row on top 
            if (!invertedInfo[1])
            {
                height -= 0.5f;
            }
        }
        heightTMP.text = "height: " + height + " cm";
    }
}
