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
        rowsInput.text = "1";
        amountPerRowInput.text = "10";
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
        
        if (howManyRows < 1 || howManyRows > 30)
        {
            ShowErrorMessage("Rows have a minimum value of 1 and a maximum of 30.");
        }
        else if (amountPerRow < 10 || amountPerRow > 50)
        {
            ShowErrorMessage("At least 10 and at most 50 pieces per row are required.");
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
                    rowsInfo[i] = 1;
                }
                else
                {
                    rowsInfo[i] = 0; 
                }
            }
            generator.GenerateCylinder(rowsInfo, amountPerRow/*, collapsed*/);
            calculator.CalculateDimensions(rowsInfo, amountPerRow);
            widthTMP.text = "Width: " + calculator.GetWidth() + " cm";
            heightTMP.text = "Height: " + calculator.GetHeight() + " cm";
        }
    }

    private void ShowErrorMessage(string errorDescription)
    {
        errorPanel.SetActive(true);
        errorText.text = errorDescription;
    }
    
}
