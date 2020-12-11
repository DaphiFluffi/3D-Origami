using System;
using System.Collections;
using System.Collections.Generic;
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
    private TMP_Text errorText;
    void Awake()
    {
        errorText = errorPanel.transform.Find("ErrorMessage").GetComponent<TMP_Text>();
        errorPanel.SetActive(false);
    }
    
    private void ShowErrorMessage(string errorDescription)
    {
        errorPanel.SetActive(true);
        errorText.text = errorDescription;
        // disable "Generate" Button
        generateButton.enabled = false;
        generateButton.GetComponent<Image>().color = Color.gray;
    }

    private void HideErrorMessage()
    {
        // hide error panel
        errorPanel.SetActive(false);
        // re-enable "Generate" Button
        generateButton.enabled = true;
        generateButton.GetComponent<Image>().color = Color.white; 
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
        if (rowInt == 1)
        {
            whereToAddDecreasedRow.GetComponentInChildren<TMP_Text>().color = Color.gray;
            whereToAddDecreasedRow.GetComponentInChildren<Image>().color = Color.gray;
            whereToAddDecreasedRow.enabled = false; 
        }
        else
        {
            whereToAddDecreasedRow.GetComponentInChildren<TMP_Text>().color = Color.white;
            whereToAddDecreasedRow.GetComponentInChildren<Image>().color = Color.white;
            whereToAddDecreasedRow.enabled = true; 
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
            ShowErrorMessage("You can only use decrease rows that are multiples of 3.");
            whereToAddDecreasedRow.GetComponentInChildren<TMP_Text>().color = Color.gray;
            whereToAddDecreasedRow.GetComponentInChildren<Image>().color = Color.gray;
            whereToAddDecreasedRow.enabled = false; 
        }
        else
        {
            HideErrorMessage();
            whereToAddDecreasedRow.GetComponentInChildren<TMP_Text>().color = Color.white;
            whereToAddDecreasedRow.GetComponentInChildren<Image>().color = Color.white;
            whereToAddDecreasedRow.enabled = true; 
        }
    }
    
    public void CheckIncreasedAndDecreasedInput(string increasedOrDecreasedString)
    {
        // https://stackoverflow.com/questions/17472580/regular-expression-to-allow-comma-and-space-delimited-number-list
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
}
