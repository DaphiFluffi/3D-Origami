using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        rowsInput.text = "1";
        amountPerRowInput.text = "9";
    }
    
    public void OnSubmit()
    {
        // does not work 
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
        
        // TODO this is wrong at this point 
        //TODO all the error messaging etc. should happen on EndEdit so that you instantly know about your mistakes rather than hvong to generate something false 
        if (howManyRows == 1)
        {
            whereToAddDecreasedRow.enabled = false; 
            whereToAddDecreasedRow.GetComponentInChildren<TMP_Text>().color = Color.gray;
        }
        
        if (howManyRows < 1 || howManyRows > 30)
        {
            ShowErrorMessage("Rows have a minimum value of 1 and a maximum of 30.");
        }
        else if (amountPerRow < 9 || amountPerRow > 50)
        {
            ShowErrorMessage("At least 9 and at most 50 pieces per row are required.");
        }
        else
        {
            errorPanel.SetActive(false);
            rowsInfo = new int[howManyRows];
            int[] invertedRowsArray = Array.ConvertAll<string, int>(whereToAddInvertedRow.text.Split(','), int.Parse); 
            List<int> invertedRowsList = new List<int>(invertedRowsArray); //converted array to a list 
            for (int i = 0; i < rowsInfo.Length; i++)
            {
                if (invertedRowsList.Contains(0))
                {
                    Array.Clear(rowsInfo, 0, rowsInfo.Length);
                }
                else if (invertedRowsList.Contains(i + 1))
                {
                   // rowsInfo[i] = 1;
                   rowsInfo[i] = 2;
                }
                else
                {
                    rowsInfo[i] = 0; 
                }
            }

            // rowsInfo[1] = 2;
            generator.GenerateCylinder(rowsInfo, amountPerRow/*, collapsed*/);
            calculator.CalculateDimensions(rowsInfo, amountPerRow);
            widthTMP.text = "Width: " + calculator.GetWidth() + " cm";
            heightTMP.text = "Height: " + calculator.GetHeight() + " cm";
        }
    }

    public void CheckRowsInput(string rowString)
    {
        int howManyRows = int.Parse(rowString);
        if (howManyRows < 1 || howManyRows > 30)
        {
            ShowErrorMessage("Rows have a minimum value of 1 and a maximum of 30.");
        }
        else
        {
            errorPanel.SetActive(false);
        }
        if (howManyRows == 1)
        {
            whereToAddDecreasedRow.GetComponentInChildren<TMP_Text>().color = Color.gray;
            whereToAddDecreasedRow.enabled = false; 
        }
    }
    private void ShowErrorMessage(string errorDescription)
    {
        errorPanel.SetActive(true);
        errorText.text = errorDescription;
    }
    
}
