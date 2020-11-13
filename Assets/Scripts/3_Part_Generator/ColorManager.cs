using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    [SerializeField] private Image paintPotPrefab = default;
    private int amount = 1;
    private Vector3 spawnPosition;
    private GameObject palettePanel;
    private Image paintPot;
    private Color colorWeAreCurrentlyCounting;
    private Dictionary<Color, int> usedColorsAndAmounts;
    
    void Awake()
    {
        palettePanel = GameObject.FindGameObjectWithTag("Palette");
        //paintPot = palettePanel.transform.GetChild(0).GetComponent<Image>();
        spawnPosition = palettePanel.transform.position;
        usedColorsAndAmounts = new Dictionary<Color, int>();
    }
    
    //called every time a color changes
    public void HowManyPiecesAreTheSameColor(Color beforeColor, Color afterColor)
    {
        amount++;
        //https://docs.unity3d.com/ScriptReference/ColorUtility.ToHtmlStringRGB.html?_ga=2.191917357.688254702.1604789679-1992563851.1586013854
        
        //save colors we had before to an array and ask the array if we had the color already
        
        colorWeAreCurrentlyCounting = afterColor; //keep track of current color

        if (!usedColorsAndAmounts.ContainsKey(colorWeAreCurrentlyCounting)) //color NOT in database yet
        {
            Debug.Log("new");
            
            usedColorsAndAmounts.Add(colorWeAreCurrentlyCounting, amount); // add to database
            InstantiatePaintPot(colorWeAreCurrentlyCounting); //instantiate a paintpot for new color
            amount = 1; 
        }
        else
        {
            Debug.Log("old");
            usedColorsAndAmounts[colorWeAreCurrentlyCounting] = amount;
            paintPot = GameObject.Find(ColorUtility.ToHtmlStringRGB(colorWeAreCurrentlyCounting)).GetComponent<Image>();
        }

        paintPot.GetComponentInChildren<TMP_Text>().text = amount.ToString();
    }
    // Debug.Log("My old color was " + ColorUtility.ToHtmlStringRGB(beforeColor) + " now I am " + ColorUtility.ToHtmlStringRGB(afterColor));
   // dictionary [color1][amount: 44]
   // Als aller aller erstes ist jedes Teil im gesamten Modell weiß -> Counter = rows*PiecesPErRow 3*9 = 27 Teile
   // did we get this color for the first time?
   // yes: arrange new counter for it
   // no: ++ the old counter
   // what was the color before the change? 
   // -- the counter for that color
   // extra funktion: erstelle color palette für den user
   // sortiere die Farben nach Menge
   // Zeige die Aktuelle Menge an

   private void InstantiatePaintPot(Color newColor)
   {
       //instantiate a new paint pot with the color and the amoount
       paintPot = Instantiate(paintPotPrefab, spawnPosition, Quaternion.identity);
       paintPot.color = newColor;
       paintPot.name = ColorUtility.ToHtmlStringRGB(newColor);
       paintPot.transform.SetParent(palettePanel.transform);
       spawnPosition += new Vector3(55, 0, 0);
   }
}
