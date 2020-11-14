using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    [SerializeField] private Image paintPotPrefab = default;
    [SerializeField] private TMP_InputField rowsInput = default;
    [SerializeField] private TMP_InputField amountPerRowInput = default;
    private int amount = 1;
    private Vector3 spawnPosition;
    private GameObject palettePanel;
    private Image paintPot;
   // private Color colorWeAreCurrentlyCounting;
    private Dictionary<string, int> usedColorsAndAmounts;
    
    void Awake()
    {
        int rows = int.Parse(rowsInput.text);
        int amountPerRow = int.Parse(amountPerRowInput.text);
        int totalRows = rows * amountPerRow;
        palettePanel = GameObject.FindGameObjectWithTag("Palette");
        //paintPot = palettePanel.transform.GetChild(0).GetComponent<Image>();
        spawnPosition = palettePanel.transform.position;
        // when it used to be <Color,int> it did not work 
        usedColorsAndAmounts = new Dictionary<string, int>();
        // start with Counter = rows*PiecesPErRow 3*9 = 27 Teile WEISS
        usedColorsAndAmounts.Add("FFFFFF", totalRows); // add to database
        InstantiatePaintPot(Color.white);
        paintPot.GetComponentInChildren<TMP_Text>().text = totalRows.ToString();
    }
    
    //called every time a color changes
    public void HowManyPiecesAreTheSameColor(Color beforeColor, Color afterColor)
    {
        string afterColorHex = ColorUtility.ToHtmlStringRGB(afterColor);
        string beforeColorHex = ColorUtility.ToHtmlStringRGB(beforeColor);
        //https://docs.unity3d.com/ScriptReference/ColorUtility.ToHtmlStringRGB.html?_ga=2.191917357.688254702.1604789679-1992563851.1586013854
        if (!usedColorsAndAmounts.ContainsKey(afterColorHex))
        {
            amount = 1;
            usedColorsAndAmounts.Add(afterColorHex, amount); // add to database
            //instantiate a paintpot for new color
            InstantiatePaintPot(afterColor); 
            paintPot.GetComponentInChildren<TMP_Text>().text = amount.ToString();
        }

        else if (usedColorsAndAmounts.ContainsKey(afterColorHex))
        {
            usedColorsAndAmounts[afterColorHex] += 1;
            //find the existing paint pot with that color
            paintPot = GameObject.Find(afterColorHex).GetComponent<Image>();
            paintPot.GetComponentInChildren<TMP_Text>().text = usedColorsAndAmounts[afterColorHex].ToString();
        }

        if (usedColorsAndAmounts.ContainsKey(beforeColorHex)) //if the beforeColor was in the database, it was overcolored -> -1
        {
            usedColorsAndAmounts[beforeColorHex] -= 1;
            //find the existing paint pot with that color
            paintPot = GameObject.Find(beforeColorHex).GetComponent<Image>();
            paintPot.GetComponentInChildren<TMP_Text>().text = usedColorsAndAmounts[beforeColorHex].ToString();
            //if we reach 0 -> remove paintPot, also remove key from database 
            /*if (usedColorsAndAmounts[beforeColorHex] == 0)
            {
                spawnPosition -= new Vector3(55, 0, 0);
                usedColorsAndAmounts.Remove(beforeColorHex);
                Destroy(paintPot.gameObject);
            }*/
        }
        
        foreach (KeyValuePair<string, int> kvp in usedColorsAndAmounts)
        {
            Debug.Log(kvp.Key + "" + kvp.Value);
        }
       
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
