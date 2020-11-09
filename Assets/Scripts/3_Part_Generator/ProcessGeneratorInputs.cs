using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Debug = UnityEngine.Debug;

public class ProcessGeneratorInputs : MonoBehaviour
{
    [SerializeField] private TMP_InputField rowsInput = default;
    [SerializeField] private TMP_InputField amountPerRowInput = default;
    [SerializeField] private GameObject origamiObject = default;
    [SerializeField] private TMP_InputField whereToAddInvertedRow = default;
    [SerializeField] private TMP_InputField whereToAddDecreasedRow = default;
    [SerializeField] private TMP_Text widthTMP = default;
    [SerializeField] private TMP_Text heightTMP = default;
    
    private CircleGenerator generator;
    private CalculateWidthHeight calculator;
    //private TMP_Text errorText;
    private int howManyRows;
    private int[] rowsInfo;
    private int amountPerRow;
    void Awake()
    {
        generator = origamiObject.GetComponent<CircleGenerator>();
        calculator = origamiObject.GetComponent<CalculateWidthHeight>();
        // generates the most basic, but stable, base 3D Origami model
        rowsInput.text = "3";
        amountPerRowInput.text = "9";
        // prevents false input exception
        whereToAddInvertedRow.text = "0"; 
        whereToAddDecreasedRow.text = "0";
        
    }
    
    public void OnSubmit()
    {
        // TODO does not work 
        // https://answers.unity.com/questions/1151762/check-if-inputfield-is-empty.html
        /*if (string.IsNullOrEmpty(rowsInput.text) || string.IsNullOrEmpty(amountPerRowInput.text))
        {
            ShowErrorMessage("Please provide values for all input fields.", true);
        }
        else
        {*/
            howManyRows = int.Parse(rowsInput.text);
            amountPerRow = int.Parse(amountPerRowInput.text);
        //}
        
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
    }
}
