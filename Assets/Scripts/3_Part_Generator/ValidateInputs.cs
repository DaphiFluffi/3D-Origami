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
    
    private void ShowErrorMessage(string errorDescription, bool enable)
    {
        if (enable)
        {
            errorPanel.SetActive(true);
            errorText.text = errorDescription;
            // disable "Generate" Button
            generateButton.enabled = false;
            generateButton.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            // hide error panel
            errorPanel.SetActive(false);
            // re-enable "Generate" Button
            generateButton.enabled = true;
            generateButton.GetComponent<Image>().color = Color.white; 
        }
    }

    private void InputFieldsCantBeEmpty(string inputString)
    {
        try
        {
            if(string.IsNullOrEmpty(inputString))
            {
                ShowErrorMessage("Please provide values for all input fields.", true);
            }
            else
            {
                ShowErrorMessage("Please provide values for all input fields.", false);
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
        if (rowInt < 1 || rowInt > 30)
        {
            ShowErrorMessage("Rows have a minimum value of 1 and a maximum of 30.", true);
        }
        else
        {
            ShowErrorMessage("Rows have a minimum value of 1 and a maximum of 30.", false);
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
            ShowErrorMessage("At least 9 and at most 50 pieces per row are required.", true);
        }
        else
        {
            ShowErrorMessage("At least 9 and at most 50 pieces per row are required.", false);
        }
        // only allow decreasing for rows that are multiples of 3
        if (amountInt % 3 > 0)
        {
            //TODO this error message always dominates the other one 
           // ShowErrorMessage("You can only use decrease in rows that are multiples of 3.");
            whereToAddDecreasedRow.GetComponentInChildren<TMP_Text>().color = Color.gray;
            whereToAddDecreasedRow.GetComponentInChildren<Image>().color = Color.gray;
            whereToAddDecreasedRow.enabled = false; 
        }
        else
        {
           // errorPanel.SetActive(false);
            whereToAddDecreasedRow.GetComponentInChildren<TMP_Text>().color = Color.white;
            whereToAddDecreasedRow.GetComponentInChildren<Image>().color = Color.white;
            whereToAddDecreasedRow.enabled = true; 
        }
    }
    
    public void CheckInvertedAndDecreasedInput(string invertedOrDecreasedString)
    {
        // https://stackoverflow.com/questions/17472580/regular-expression-to-allow-comma-and-space-delimited-number-list
        // only matches series of natural numbers with commas in between
        // @ is to skip over the escape character "\"
        Regex rgx = new Regex(@"^[\d,\s]+$");
        if(rgx.IsMatch(invertedOrDecreasedString))
        {
            ShowErrorMessage("Please type in the rows' numbers separated by commas.", false);
            // https://stackoverflow.com/questions/47646090/int-parse-is-not-working-with-string-value-system-formatexception-input-string
            try
            {
                int rows = int.Parse(rowsInput.text);
                //TODO TryParse does not work with arrays(?)
                int[] invertedArray = Array.ConvertAll<string, int>(invertedOrDecreasedString.Split(','), int.Parse);
                //TODO first row cannot be decreased
                for(int i = 0; i < invertedArray.Length; i++)
                {
                    if (invertedArray[i] > rows)
                    {
                        ShowErrorMessage("You cannot access a row that has not been generated.", true);
                    }
                    else
                    {
                        ShowErrorMessage("You cannot access a row that has not been generated.", false);
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
         ShowErrorMessage(" Please type in the rows' numbers separated by commas.", true);
        }
    }
}
