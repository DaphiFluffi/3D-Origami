using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValidateInputs : MonoBehaviour
{
    [SerializeField] private TMP_InputField rowsInput = default;
    [SerializeField] private TMP_InputField whereToAddDecreasedRow = default;
    [SerializeField] private Button generateButton = default; 
    [SerializeField] private GameObject errorPanel = default;
    [SerializeField] private GameObject infoPanel = default;
    private TMP_Text errorText;
    private TMP_Text infoText;
    private int[] increasedArray;
    private int[] decreasedArray;

    void Awake()
    {
        errorText = errorPanel.transform.Find("ErrorMessage").GetComponent<TMP_Text>();
        infoText = infoPanel.transform.Find("InfoMessage").GetComponent<TMP_Text>();
        errorPanel.SetActive(false);
    }

    private void ShowInfoMessage(string infoDescription)
    {
        infoPanel.SetActive(true);
        infoText.text = infoDescription;
    }
    
    private void ShowErrorMessage(string errorDescription)
    {
        errorPanel.SetActive(true);
        errorText.text = errorDescription;
        // disable "Generate" Button
        generateButton.enabled = false;
        generateButton.GetComponent<Image>().color = Color.gray;
    }
    
    public void HideInfoMessage()
    {
        infoPanel.SetActive(false);
    }

    private void HideErrorMessage()
    {
        // hide error panel
        errorPanel.SetActive(false);
        // re-enable "Generate" Button
        generateButton.enabled = true;
        generateButton.GetComponent<Image>().color = Color.white; 
    }

    public void HideErrorPanel()
    {
        errorPanel.SetActive(false);
    }
    
    private void InputFieldsCantBeEmpty(string inputString)
    {
        try
        {
            if(string.IsNullOrEmpty(inputString))
            {
                ShowErrorMessage("Please provide values for all input fields.");
            }
            else
            {
                HideErrorMessage();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    public void CheckRowsInput(string rowString)
    {
        InputFieldsCantBeEmpty(rowString);
        int rowInt = int.Parse(rowString);
        if (rowInt < 2 || rowInt > 30)
        {
            ShowErrorMessage("Rows have a minimum value of 2 and a maximum of 30.");
        }
        else
        {
            HideErrorMessage();
        }
    }

    public void CheckAmountInput(string amountString)
    {
        InputFieldsCantBeEmpty(amountString);
        int amountInt = int.Parse(amountString);
        if (amountInt < 9 || amountInt > 50)
        {
            ShowErrorMessage("At least 9 and at most 50 pieces per row are required.");
        }
        // only allow decreasing for rows that are multiples of 3
        else if (amountInt % 3 > 0)
        {
            ShowInfoMessage("You can only decrease rows that are divisible 3.");
            whereToAddDecreasedRow.GetComponentInChildren<TMP_Text>().color = Color.gray;
            whereToAddDecreasedRow.GetComponentInChildren<Image>().color = Color.gray;
            whereToAddDecreasedRow.enabled = false; 
        }
        else
        {
            HideInfoMessage();
            HideErrorMessage();
            whereToAddDecreasedRow.GetComponentInChildren<TMP_Text>().color = Color.white;
            whereToAddDecreasedRow.GetComponentInChildren<Image>().color = Color.white;
            whereToAddDecreasedRow.enabled = true; 
        }
    }
    
    private void CheckIncreasedAndDecreasedInput(string increasedOrDecreasedString)
    {
        // https://stackoverflow.com/a/17472609
        // only matches series of natural numbers with commas in between
        // @ is to skip over the escape character "\"
        Regex rgx = new Regex(@"^[\d,\s]+$");
        if(rgx.IsMatch(increasedOrDecreasedString))
        {
            HideErrorMessage();
            // https://stackoverflow.com/questions/47646090/int-parse-is-not-working-with-string-value-system-formatexception-input-string
            try
            {
                int rows = int.Parse(rowsInput.text);
                int[] increasedOrDecreasedArray = Array.ConvertAll<string, int>(increasedOrDecreasedString.Split(','), int.Parse);
                for(int i = 0; i < increasedOrDecreasedArray.Length; i++)
                {
                    if (increasedOrDecreasedArray[i] > rows)
                    {
                        ShowErrorMessage("You cannot access a row that has not been generated.");
                    }
                    else if (increasedOrDecreasedArray[i] == 1)
                    {
                        ShowErrorMessage("First row cannot be de- or increased.");
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

    public void CheckIncreasedInput(string increasedString)
    {
        CheckIncreasedAndDecreasedInput(increasedString);
        int[] increasedArray =  Array.ConvertAll<string, int>(increasedString.Split(','), int.Parse);
        this.increasedArray = increasedArray;
        if (decreasedArray != null)
        {
            CheckDoubleInput();
        }
    }
    
    public void CheckDecreasedInput(string decreasedString)
    {
        CheckIncreasedAndDecreasedInput(decreasedString);
        int[] decreasedArray =  Array.ConvertAll<string, int>(decreasedString.Split(','), int.Parse);
        this.decreasedArray = decreasedArray;
        if (increasedArray != null)
        {
            CheckDoubleInput();
        }
    }

    private void CheckDoubleInput()
    {
        // https://stackoverflow.com/a/55215794
        // 0 and 0 does not count as doubles
        // check if there is equal items
        if (increasedArray.Intersect(decreasedArray).Any() && increasedArray[0] != 0 && decreasedArray[0] != 0) 
        {
            // the intersection
            var equalItems = increasedArray.Intersect(decreasedArray);
            //https://stackoverflow.com/a/5079888
            string errorString = String.Join(",", equalItems);
            ShowErrorMessage("The row(s) no." + errorString + " cannot be increased and decreased at the same time.");
        }
    }
}
