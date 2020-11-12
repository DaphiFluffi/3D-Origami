using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    [SerializeField] private Image paintPotPrefab = default;
    private int counter;
    private GameObject palettePanel;
    private Image paintPot;
    private TMP_Text amount;
    private Color colorWeAreCurrentlyCounting;
    void Awake()
    {
        palettePanel = GameObject.FindGameObjectWithTag("Palette");
        paintPot = palettePanel.transform.GetChild(0).GetComponent<Image>();
    }
    
   public void HowManyPiecesAreTheSameColor(Color colorWeGot)
   {
        Image paintPot;
        counter++;
        /*Debug.Log(counter);
        Debug.Log(colorWeGot);*/
        if (colorWeAreCurrentlyCounting != colorWeGot)
        {
            colorWeAreCurrentlyCounting = colorWeGot;
            //instantiate a new paint pot with the color
            Debug.Log("We changed the color. We should count it separately.");

            paintPot = Instantiate(paintPotPrefab, palettePanel.transform.position, Quaternion.identity);
            paintPot.color = colorWeGot;
            paintPot.name = "paintPotFor " + colorWeGot;
            // parenting every piece to its respective row 
            paintPot.transform.SetParent(palettePanel.transform);
       
        }
        else
        {
            // kinda just counting on that panel by i'll take it for now
            paintPot = this.paintPot;
        }
        paintPot.GetComponentInChildren<TMP_Text>().text = counter.ToString();

        // Als aller aller erstes ist jedes Teil im gesamten Modell weiß -> Counter = rows*PiecesPErRow
        //did we get this color for the first time?
        // yes: arrange new counter for it
        // no: ++ the old counter
        // what was the color before the change? 
        // -- the counter for that color
        // extra funktion: erstelle color palette für den user
        // sortiere die Farben nach Menge
        //Zeige die Aktuelle Menge an
    }
}
