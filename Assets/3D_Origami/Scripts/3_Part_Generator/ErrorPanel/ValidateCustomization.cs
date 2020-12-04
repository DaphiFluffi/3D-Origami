using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class ValidateCustomization : MonoBehaviour
{
    [SerializeField] private GameObject errorPanel = default;
    private TMP_Text errorText;
    private GameObject[] generatedRows;
    // Start is called before the first frame update
    void Awake()
    {
        errorText = errorPanel.transform.Find("ErrorMessage").GetComponent<TMP_Text>();
        errorPanel.SetActive(false);
    }
    private void ShowErrorMessage(string errorDescription)
    {
        errorPanel.SetActive(true);
        errorText.text = errorDescription;
    }
    
    private void HideErrorMessage()
    {
        errorPanel.SetActive(false);
    }
    
    public void CheckAdd()
    {
        generatedRows= GameObject.FindGameObjectsWithTag("Row");
        if (generatedRows.Length == 30)
        {
            ShowErrorMessage("There can be a maximum of 30 rows.");
        }
    }

    public void CheckRemove()
    {
        generatedRows = GameObject.FindGameObjectsWithTag("Row");
        if (generatedRows.Length == 2)
        {
            ShowErrorMessage("There can be a minimum of 2 rows.");
        }
    }
    
    public void CheckInvertedInput(string invertedString)
    {
        generatedRows = GameObject.FindGameObjectsWithTag("Row");
        // https://stackoverflow.com/questions/17472580/regular-expression-to-allow-comma-and-space-delimited-number-list
        // only matches series of natural numbers with commas in between
        // @ is to skip over the escape character "\"
        Regex rgx = new Regex(@"^[\d,\s]+$");
        if(rgx.IsMatch(invertedString))
        {
            HideErrorMessage();
            // https://stackoverflow.com/questions/47646090/int-parse-is-not-working-with-string-value-system-formatexception-input-string
            try
            {
                int[] invertedArray = Array.ConvertAll<string, int>(invertedString.Split(','), int.Parse);
                for(int i = 0; i < invertedArray.Length; i++)
                {
                    if (invertedArray[i] > generatedRows.Length)
                    {
                        ShowErrorMessage("You cannot access a row that has not been generated.");
                    }
                    else
                    {
                        HideErrorMessage();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        else
        {
            ShowErrorMessage(" Please type in the rows' numbers separated by commas.");
        }
    }
}
