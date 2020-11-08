using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Debug = UnityEngine.Debug;

public class GeneratorInputs : MonoBehaviour
{
    [SerializeField] private TMP_InputField rowsInput = default;
    [SerializeField] private TMP_InputField amountPerRowInput = default;
    [SerializeField] private GameObject errorPanel = default;
    [SerializeField] private GameObject origamiObject = default;
    [SerializeField] private TMP_InputField whereToAddInvertedRow = default;
    [SerializeField] private TMP_InputField whereToAddDecreasedRow = default;
    [SerializeField] private TMP_Text widthTMP = default;
    [SerializeField] private TMP_Text heightTMP = default;
    [SerializeField] private Button generateButton = default; 
    
    private CircleGenerator generator;
    private CalculateWidthHeight calculator;
    private TMP_Text errorText;
    private int howManyRows;
    private int[] rowsInfo;
    private int amountPerRow;
    void Awake()
    {
        generator = origamiObject.GetComponent<CircleGenerator>();
        calculator = origamiObject.GetComponent<CalculateWidthHeight>();
        errorText = errorPanel.transform.Find("ErrorMessage").GetComponent<TMP_Text>();
        errorPanel.SetActive(false);
        whereToAddInvertedRow.text = "0"; // prevents false input exception
        whereToAddDecreasedRow.text = "0";
        // generates the most basic, but stable, base 3D Origami model
        rowsInput.text = "3";
        amountPerRowInput.text = "9";
    }
    
    public void OnSubmit()
    {
        // TODO does not work 
        // https://answers.unity.com/questions/1151762/check-if-inputfield-is-empty.html
        if (string.IsNullOrEmpty(rowsInput.text) || string.IsNullOrEmpty(amountPerRowInput.text))
        {
            ShowErrorMessage("Please provide values for all input fields.");
        }
        else
        {
            howManyRows = int.Parse(rowsInput.text);
            amountPerRow = int.Parse(amountPerRowInput.text);
        }
        
        /*if (howManyRows < 1 || howManyRows > 30)
        {
            ShowErrorMessage("Rows have a minimum value of 1 and a maximum of 30.");
        }
        else if (amountPerRow < 9 || amountPerRow > 50)
        {
            ShowErrorMessage("At least 9 and at most 50 pieces per row are required.");
        }
        else
        {*/
            //errorPanel.SetActive(false);
            rowsInfo = new int[howManyRows];
            int[] invertedRowsArray = Array.ConvertAll<string, int>(whereToAddInvertedRow.text.Split(','), int.Parse); 
            List<int> invertedRowsList = new List<int>(invertedRowsArray); //converted array to a list 
            int[] decreasedRowsArray = Array.ConvertAll<string, int>(whereToAddDecreasedRow.text.Split(','), int.Parse); 
            List<int> decreasedRowsList = new List<int>(decreasedRowsArray); //converted array to a list 
            
            // in case no special rows are requested
            if (invertedRowsList.Contains(0) && decreasedRowsList.Contains(0)) 
            {
                Array.Clear(rowsInfo, 0, rowsInfo.Length);
            }
            else
            {
                for (int i = 0; i < rowsInfo.Length; i++)
                {
                    if (invertedRowsList.Contains(i + 1))
                    {
                        // 1 = inverted
                        rowsInfo[i] = 1;
                    }
                    else if (decreasedRowsList.Contains(i + 1))
                    {
                        // 2 = decreased
                        rowsInfo[i] = 2;
                    }
                    else
                    {
                        // 0 = normal
                        rowsInfo[i] = 0;
                    }
                }
            }

            //TODO make function access not public
            generator.GenerateCylinder(rowsInfo, amountPerRow);
            calculator.CalculateDimensions(rowsInfo, amountPerRow);
            widthTMP.text = "Width: " + calculator.GetWidth() + " cm";
            heightTMP.text = "Height: " + calculator.GetHeight() + " cm";
        /*}*/
    }

    //TODO validate decreased and increased input fields: rows that were not generated cannot be inverted
    //TODO create ValidationScript
    public void CheckRowsInput(string rowString)
    {
        int rowInt = int.Parse(rowString);
        if (rowInt < 1 || rowInt > 30)
        {
            ShowErrorMessage("Rows have a minimum value of 1 and a maximum of 30.");
            //disable "Generate" Button 
            generateButton.enabled = false;
            generateButton.GetComponent<Image>().color = Color.gray; 
        }
        else
        {
            generateButton.enabled = true;
            //re-enable "Generate" Button 
            generateButton.GetComponent<Image>().color = Color.white; 
            errorPanel.SetActive(false);
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
        int amountInt = int.Parse(amountString);
        if (amountInt < 9 || amountInt > 50)
        {
            ShowErrorMessage("At least 9 and at most 50 pieces per row are required.");
            //disable "Generate" Button 
            generateButton.enabled = false;
            generateButton.GetComponent<Image>().color = Color.gray; 
        }
        else
        {
            generateButton.enabled = true;
            //re-enable "Generate" Button 
            generateButton.GetComponent<Image>().color = Color.white; 
            errorPanel.SetActive(false);
        }
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
    private void ShowErrorMessage(string errorDescription)
    {
        errorPanel.SetActive(true);
        errorText.text = errorDescription;
    }
    
}
