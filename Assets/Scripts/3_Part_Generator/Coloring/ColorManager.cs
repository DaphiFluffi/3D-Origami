using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    [SerializeField] private Image paintPotPrefab = default;
    //private ProcessGeneratorInputs processGeneratorInputs;
    private int amount;
    private Vector3 spawnPosition;
    private GameObject palettePanel = default;
    private Image paintPot;
    private Dictionary<string, int> usedColorsAndAmounts;
    
    void Start()
    {
        //TODO calculate the total rows in generatorinputs and access it here ! "GetTotalRows" in STart oder so
        //processGeneratorInputs = FindObjectOfType<Canvas>().GetComponentInChildren<ProcessGeneratorInputs>();
        
        palettePanel = GameObject.FindGameObjectWithTag("Palette");
        palettePanel.SetActive (false);

        spawnPosition = palettePanel.transform.position;
        // TODO why? when it used to be <Color,int> it did not work 
        usedColorsAndAmounts = new Dictionary<string, int>();
        
    }
    
    //called every time a color changes
    public void HowManyPiecesAreTheSameColor(string beforeColor, string afterColor)
    {
        //https://docs.unity3d.com/ScriptReference/ColorUtility.ToHtmlStringRGB.html?_ga=2.191917357.688254702.1604789679-1992563851.1586013854
        if (!usedColorsAndAmounts.ContainsKey(afterColor))
        {
            amount = 1;
            usedColorsAndAmounts.Add(afterColor, amount); // add to database
            //instantiate a paintPot for new color
            InstantiatePaintPot(afterColor); 
            paintPot.GetComponentInChildren<TMP_Text>().text = amount.ToString();
        }
        else if (usedColorsAndAmounts.ContainsKey(afterColor))
        {
            usedColorsAndAmounts[afterColor] += 1;
            //find the existing paint pot with that color
            paintPot = GameObject.Find(afterColor).GetComponent<Image>();
            paintPot.GetComponentInChildren<TMP_Text>().text = usedColorsAndAmounts[afterColor].ToString();
        }

        if (usedColorsAndAmounts.ContainsKey(beforeColor)) //if the beforeColor was in the database, it was overcolored -> -1
        {
            usedColorsAndAmounts[beforeColor] -= 1;
            //find the existing paint pot with that color
            paintPot = GameObject.Find(beforeColor).GetComponent<Image>();
            paintPot.GetComponentInChildren<TMP_Text>().text = usedColorsAndAmounts[beforeColor].ToString();
            
            //if we reach 0 -> remove paintPot, also remove key from database 
            if (usedColorsAndAmounts[beforeColor] == 0)
            {
                spawnPosition = paintPot.transform.position;
                usedColorsAndAmounts.Remove(beforeColor);
                Destroy(paintPot.gameObject);
            }
        }
    }
    
   // extra funktion: erstelle color palette für den user
   // sortiere die Farben nach Menge

   private void InstantiatePaintPot(string newColor)
   {
       //instantiate a new paint pot with the color and the amoount
       paintPot = Instantiate(paintPotPrefab, spawnPosition, Quaternion.identity);
       ColorUtility.TryParseHtmlString("#"+newColor, out var convertedColor);
       paintPot.color = convertedColor;
       paintPot.name = newColor;
       paintPot.transform.SetParent(palettePanel.transform);
       spawnPosition += new Vector3(55, 0, 0);
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
       usedColorsAndAmounts.Clear();
   }

}
