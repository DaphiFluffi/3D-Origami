using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class ColorManager : MonoBehaviour
{
    [SerializeField] private Image paintPotPrefab = default;
    //private ProcessGeneratorInputs processGeneratorInputs;
    
    private GameObject content = default;
    private GameObject scrollView = default;
    private Image paintPot;
    
    private Dictionary<string, int> usedColors;
    private List<Image> paintPots;

    private int amount;
    private int totalAmount;
    void Start()
    {
        // TODO why? when it used to be <Color,int> it did not work 
        usedColors = new Dictionary<string, int>();
        paintPots = new List<Image>();
        content = GameObject.FindGameObjectWithTag("Content");
        scrollView = GameObject.FindGameObjectWithTag("Palette");
        //scrollView.SetActive(false);
    }
    
    public void InputCallback(int totalPieces)
    {
        //Debug.Log("This is the callback function working: " + totalPieces);
        usedColors.Add("FFFFFF", totalPieces);
        //instantiate a paintPot for new color
        InstantiatePaintPot("FFFFFF");
        paintPot.GetComponentInChildren<TMP_Text>().text = totalPieces.ToString();
        //Debug.Log(usedColors["FFFFFF"]);
        //HowManyPiecesAreTheSameColor("000000", "FFFFFF");

        if (usedColors.ContainsKey("FFFFFF"))
        {
            Debug.Log("InputCallback: " + usedColors["FFFFFF"]);

        }
        else
        {
            throw new Exception(String.Format("Key {0} was not found", "FFFFFF"));        

        }
        
    }
    
    //called every time a color changes
    public void HowManyPiecesAreTheSameColor(string beforeColor, string afterColor)
    {
        //Debug.Log(beforeColor + " + " + afterColor);
        //https://docs.unity3d.com/ScriptReference/ColorUtility.ToHtmlStringRGB.html?_ga=2.191917357.688254702.1604789679-1992563851.1586013854
        // if the color is used for the first time
        
        
        if (!usedColors.ContainsKey(afterColor))
        {
            Debug.Log("new");
           
            amount = 1;
            
            // add to database
            usedColors.Add(afterColor, amount); 
            //instantiate a paintPot for new color
            InstantiatePaintPot(afterColor); 
            paintPot.GetComponentInChildren<TMP_Text>().text = amount.ToString();
            if (usedColors.ContainsKey(afterColor))
            {
                Debug.Log("HowManyColors: " + usedColors[afterColor]);

            }
            else
            {
                throw new Exception(String.Format("Key {0} was not found", afterColor));        

            }
            if (usedColors.ContainsKey("FFFFFF"))
            {
                Debug.Log("HowManyColors: " + usedColors["FFFFFF"]);

            }
            else
            {
                throw new Exception(String.Format("Key {0} was not found", "FFFFFF"));        

            }
            
            
        }
        else if (usedColors.ContainsKey(afterColor))
        {
            Debug.Log("old");

            usedColors[afterColor] += 1;
            //find the existing paint pot with that color
            paintPot = GameObject.Find(afterColor).GetComponent<Image>();
            paintPot.GetComponentInChildren<TMP_Text>().text = usedColors[afterColor].ToString();
        }
        
        //if the beforeColor was in the database, it was overcolored -> -1
        if (usedColors.ContainsKey(beforeColor)) 
        {
            Debug.Log("overcolored");
            usedColors[beforeColor] -= 1;
            //find the existing paint pot with that color
            paintPot = GameObject.Find(beforeColor).GetComponent<Image>();
            paintPot.GetComponentInChildren<TMP_Text>().text = usedColors[beforeColor].ToString();
            
            //if we reach 0 -> remove paintPot, also remove key from database 
            if (usedColors[beforeColor] == 0)
            {
                usedColors.Remove(beforeColor);
                //brauch ich nicht mehr
                paintPots.Remove(paintPot);
                Destroy(paintPot.gameObject);
            }
        }
    }

    
    
   private void InstantiatePaintPot(string newColor)
   {
       paintPot = Instantiate(paintPotPrefab, content.transform);
       ColorUtility.TryParseHtmlString("#" + newColor, out var convertedColor);
       paintPot.color = convertedColor;
       paintPot.name = newColor;
       paintPots.Add(paintPot);
   }

   
   
   // TODO OnGenerateDeleteEverything
   public void ResetColorManager()
   {
       /*foreach (Transform child in palettePanel.transform)
       {
           if (child.GetType() == typeof(Image))
           {
               Destroy(child.gameObject);
           }
       }*/
      /* int totalRows = 0;
       usedColorsAndAmounts.Add("FFFFFF", totalRows); // add to database
       InstantiatePaintPot("FFFFFF");
       paintPot.GetComponentInChildren<TMP_Text>().text = totalRows.ToString();*/
       usedColors.Clear();
   }

}
