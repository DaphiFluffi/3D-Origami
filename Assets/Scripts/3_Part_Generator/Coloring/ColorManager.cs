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
    
    void Start()
    {
        // TODO why? when it used to be <Color,int> it did not work 
        usedColors = new Dictionary<string, int>();
        //TODO calculate the total rows in generatorinputs and access it here ! "GetTotalRows" in STart oder so
        //processGeneratorInputs = FindObjectOfType<Canvas>().GetComponentInChildren<ProcessGeneratorInputs>();
        paintPots = new List<Image>();
        content = GameObject.FindGameObjectWithTag("Content");
        scrollView = GameObject.FindGameObjectWithTag("Palette");
        scrollView.SetActive(false);
    }
    
    //called every time a color changes
    public void HowManyPiecesAreTheSameColor(string beforeColor, string afterColor)
    {
        //https://docs.unity3d.com/ScriptReference/ColorUtility.ToHtmlStringRGB.html?_ga=2.191917357.688254702.1604789679-1992563851.1586013854
        // if the color is used for the first time
        if (!usedColors.ContainsKey(afterColor))
        {
            int amount = 1;
            // add to database
            usedColors.Add(afterColor, amount); 
            //instantiate a paintPot for new color
            InstantiatePaintPot(afterColor); 
            paintPot.GetComponentInChildren<TMP_Text>().text = amount.ToString();
        }
        else if (usedColors.ContainsKey(afterColor))
        {
            usedColors[afterColor] += 1;
            //find the existing paint pot with that color
            paintPot = GameObject.Find(afterColor).GetComponent<Image>();
            paintPot.GetComponentInChildren<TMP_Text>().text = usedColors[afterColor].ToString();
        }
        
        //if the beforeColor was in the database, it was overcolored -> -1
        if (usedColors.ContainsKey(beforeColor)) 
        {
            usedColors[beforeColor] -= 1;
            //find the existing paint pot with that color
            paintPot = GameObject.Find(beforeColor).GetComponent<Image>();
            paintPot.GetComponentInChildren<TMP_Text>().text = usedColors[beforeColor].ToString();
            
            //if we reach 0 -> remove paintPot, also remove key from database 
            if (usedColors[beforeColor] == 0)
            {
                usedColors.Remove(beforeColor);
                paintPots.Remove(paintPot);
             //   ArrangePaintPots(paintPots);
                Destroy(paintPot.gameObject);
            }
        }
    }

   private void InstantiatePaintPot(string newColor)
   {
       //paintPot.transform.SetParent(palettePanel.transform);
       paintPot = Instantiate(paintPotPrefab, content.transform);
       ColorUtility.TryParseHtmlString("#"+newColor, out var convertedColor);
       paintPot.color = convertedColor;
       paintPot.name = newColor;
       paintPots.Add(paintPot);
      // ArrangePaintPots(paintPots);
   }

   // arranges paint pots 
   // used when a new paint pot is added or one is removed
   /*private void ArrangePaintPots(List<Image> paintPotsList)
   {
       paintPotsList[0].transform.position = spawnPosition;
       for (int i = 1; i < paintPotsList.Count; i++)
       {
           int margin = 5; 
           paintPotsList[i].transform.position = spawnPosition + new Vector3( i * (2 * paintPotPrefabWidth + margin),0,0);
            Debug.Log("paintPot " + paintPotsList[i].rectTransform.rect.position.x);
            Debug.Log("palette: " + paletteScrollView.GetComponent<RectTransform>().rect.width);
           /*if (paintPotsList[i].transform.position.x == palettePanel.GetComponent<RectTransform>().rect.width)
           {
               //because my paint pot is a square I can use Width, it should be height otherwise
               paintPotsList[i].transform.position += new Vector3(0,paintPotPrefabWidth + margin,0);
           }*/
       //}
   //}
   
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
