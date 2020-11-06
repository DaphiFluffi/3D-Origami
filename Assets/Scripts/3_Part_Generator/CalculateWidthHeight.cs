using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateWidthHeight : MonoBehaviour
{
    private float width;
    private float height;

    public void CalculateDimensions(int[] rows, int amountPerRow)
    {
        width = 3.4f + (0.05f * amountPerRow);
        /* 
        Höhe:normalen Reihen  = 2cm (Teil-Höhe) + (0,5 cm (1/4 der Teil-Höhe) * Menge an Reihen)
        1. Reihe eine inverted Row oder zwei aufeinanderforlgende : - 0,5 cm 
            normale Reihe - inverted Reihe - normale Reihe: +0,5 cm 
        */
        height = 2f + 0.5f * (rows.Length - 1); //for normal rows 

        if (rows[0] == 1 && rows.Length > 1) //if we have an inverted row in the first row and it is not the only row
        {
            height -= 0.5f;
        }
        for (int i = 0; i < rows.Length; i++)
        {
            if (rows[i] == 1 && i > 0 && i < rows.Length - 1) //whenever we have an inverted row in the middle, but not at the top or bottom 
            {
                height += 0.5f;
            }
          /*  else if (rows[i] == 1 && rows[i + 1] == 1 && (i+1) != rows.Length - 1)
            {
                height -= 0.5f;
            }*/
        }
        //Debug.Log("Width: " + width + " cm Height: " + height + " cm" + rows.Length);
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
