using System.Linq;
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
        widthTMP.text = "w: " + width + " cm";
        heightTMP.text = "h: " + height + " cm";
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
        heightTMP.text = "h: " + height + " cm";
    }

    public void CalculateDimensions(bool[] invertedInfo)
    {
        // if we have an inverted row in the first row and the model does not only consist of one row
        if (invertedInfo[0])
        {
            height -= 0.5f;
        }
        else
        {
            // if there are inverted rows at the top 
            if (invertedInfo[1])
            {
                height += 0.5f;
            }
        
            if (invertedInfo[2])
            {
                height -= 0.5f; 
            }
        }
        heightTMP.text = "h: " + height + " cm";
    }
}
