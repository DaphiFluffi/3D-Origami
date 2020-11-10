using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CalculateWidthHeight : MonoBehaviour
{
    private float width;
    private float height;

    public void CalculateDimensions(int[] rows, int amountPerRow)
    {
        width = 3.4f + (0.05f * amountPerRow);
        //for normal rows 
        height = 2f + 0.5f * (rows.Length - 1); 
        //Debug.Log( "normal: "  + height);
        
        //TODO comment this mess
        //whenever the first row is inverted, the model is 0.5 cm shorter
        if (rows[0] == 1 && rows[rows.Length -1] != 1)
        {
            Debug.Log("-0.5");
            height -= 0.5f;
        }
        else
        {
            if (rows.Contains(1) && rows[rows.Length - 1] == 1 && rows[rows.Length - 2] == 1)
            {
                Debug.Log("+0.5");
                height += 0.5f;
            }

            int index = 0;
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i] == 1)
                {
                    index++;
                    if (index == rows.Length)
                    {
                        height -= 0.5f; 
                    }
                }
            }
        }
    }

    public float GetWidth()
    {
        return width; 
    }

    public float GetHeight()
    {
        return height; 
    }
}
