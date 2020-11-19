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
    
    private int hashWhite;
    private int amount;
    private int totalAmount;
    void Start()
    {
        // TODO why? when it used to be <Color,int> it did not work 
        // I would have had to override Equals() function
        // for dictionaries with complex keys
        usedColors = new Dictionary<string, int>();
        content = GameObject.FindGameObjectWithTag("Content");
        scrollView = GameObject.FindGameObjectWithTag("Palette");
        //scrollView.SetActive(false);
    }
    
    //called on Generate Button
    public void InputCallback(int totalPieces)
    {
        usedColors.Add("FFFFFF", totalPieces);
        //instantiate a paintPot for white
        InstantiatePaintPot("FFFFFF");
        SetPaintPotText(totalPieces);
    }
    
    //called every time a color changes
    public void HowManyPiecesAreTheSameColor(string beforeColor, string afterColor)
    {
        //https://docs.unity3d.com/ScriptReference/ColorUtility.ToHtmlStringRGB.html?_ga=2.191917357.688254702.1604789679-1992563851.1586013854
        // if the color is used for the first time
        if (!usedColors.ContainsKey(afterColor))
        {
            // add to database
            usedColors.Add(afterColor, 1); 
            //instantiate a paintPot for new color
            InstantiatePaintPot(afterColor); 
            SetPaintPotText(1);

        }
        else if (usedColors.ContainsKey(afterColor))
        {
            usedColors[afterColor] += 1;
            //find the existing paint pot with that color
            paintPot = GameObject.Find(afterColor).GetComponent<Image>();
            SetPaintPotText(usedColors[afterColor]);

        }
        
        //if the beforeColor was in the database, it was overcolored -> -1
        if (usedColors.ContainsKey(beforeColor)) 
        {
            usedColors[beforeColor] -= 1;
            //find the existing paint pot with that color
            paintPot = GameObject.Find(beforeColor).GetComponent<Image>();
            SetPaintPotText(usedColors[beforeColor]);

            //if we reach 0 -> remove paintPot, also remove key from database 
            if (usedColors[beforeColor] == 0)
            {
                usedColors.Remove(beforeColor);
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
   }

   private void SetPaintPotText(int number)
   {
       ColorUtility.TryParseHtmlString("#" + paintPot.name, out var convertedColor);
       // https://docs.unity3d.com/ScriptReference/Color.RGBToHSV.html
       float H, S, V;
       Color.RGBToHSV(convertedColor, out H, out S, out V);
       if (V <= 0.3f)
       {
           paintPot.GetComponentInChildren<TMP_Text>().color = Color.white;
       }
       else
       {
           paintPot.GetComponentInChildren<TMP_Text>().color = Color.black;
       }
       paintPot.GetComponentInChildren<TMP_Text>().text = number.ToString();

   }
   
   public void ResetColorManager()
   {
       foreach (Transform child in content.transform)
       {
           Destroy(child.gameObject);
       }
       usedColors.Clear();
   }

}
