using System.Linq;
using UnityEngine;

public class CalculateWidthHeight : MonoBehaviour
{
    private float width;
    private float height;

    public float GetWidth()
    {
        return width; 
    }

    public float GetHeight()
    {
        return height; 
    }
    
    public void CalculateDimensions(int[] rows, int amountPerRow)
    {
        // for normal rows 
        // width is not affected by inverted rows
        width = 3.4f + (0.05f * amountPerRow);
        height = 2f + 0.5f * (rows.Length - 1); 
        
        //TODO should I allow a minimum of 2 rows
        // if we have an inverted row in the first row and the model does not only consist of one row
        if (rows[0] == 1 && rows[rows.Length -1] != 1)
        {
            height -= 0.5f;
        }
        else
        {
            // if there are inverted rows at the top 
            if (rows.Contains(1) && rows[rows.Length - 1] == 1 && rows[rows.Length - 2] == 1)
            {
                height += 0.5f;
            }

            int index = 0;
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i] == 1)
                {
                    index++;
                    //if the entire model is out of inverted rows 
                    if (index == rows.Length)
                    {
                        height -= 0.5f; 
                    }
                }
            }
        }
    }
}
