using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateWidthHeight : MonoBehaviour
{
    private float width;
    private float height;
    private bool calculated;
    public void CalculateDimensions(float howManyRows, int amountPerRow, List<int> invertedRows)
    {
        /*
        Breite:
        3,4cm + (0,05 cm * TeileProReihe)
        */
        width = 3.4f + (0.05f * amountPerRow);
        /* 
        Höhe:
        normalen Reihen  = 2cm (Teil-Höhe) + (0,5 cm (1/4 der Teil-Höhe) * Menge an Reihen)
        1. Reihe eine inverted Row oder zwei aufeinanderforlgende : - 0,5 cm 
            normale Reihe - inverted Reihe - normale Reihe: +0,5 cm 
        */
        height = 2f + 0.5f * (howManyRows - 1); //for normal rows 
        if (invertedRows.Contains(1))
        {
            height -= 0.5f;
        }
        // if we have invertedRows at all 
        else if (!invertedRows.Contains(0))
        {
            // for every inverted row like that add 0.5 cm unless we are in the top row
            foreach (int inverted in invertedRows)
            {
                if (inverted != ((int) howManyRows))
                {
                    height += 0.5f;
                }
            }
        }
        //Debug.Log("Width: " + width + " cm Height: " + height + " cm");
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
