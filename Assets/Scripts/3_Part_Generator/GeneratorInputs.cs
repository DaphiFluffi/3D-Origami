using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GeneratorInputs : MonoBehaviour
{
    [SerializeField] private TMP_InputField rowsInput = default;
    [SerializeField] private TMP_InputField amountPerRowInput = default;
    [SerializeField] private Toggle collapsedInput = default;
    [SerializeField] private GameObject errorPanel = default;
    [SerializeField] private GameObject origamiObject = default;
    [SerializeField] private TMP_InputField whereToAddInvertedRow = default;
    [SerializeField] private TMP_Text widthTMP = default;
    [SerializeField] private TMP_Text heightTMP = default;
    
        private CircleGenerator generator;
    private CalculateWidthHeight calculator;
    private TMP_Text errorText;
    private float rows;
    private int amountPerRow;
    private bool collapsed;
    void Awake()
    {
        generator = origamiObject.GetComponent<CircleGenerator>();
        calculator = origamiObject.GetComponent<CalculateWidthHeight>();
        errorText = errorPanel.transform.Find("ErrorMessage").GetComponent<TMP_Text>();
        errorPanel.SetActive(false);
        whereToAddInvertedRow.gameObject.SetActive(false);
        whereToAddInvertedRow.DeactivateInputField();
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
            rows = float.Parse(rowsInput.text);
            amountPerRow = int.Parse(amountPerRowInput.text);
        }

        collapsed = collapsedInput.isOn;
        
        
        if (rows < 1 || rows > 30)
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
            int[] invertedRowsArray = Array.ConvertAll<string, int>(whereToAddInvertedRow.text.Split(','), int.Parse); 
            List<int> invertedRowsList = new List<int>(invertedRowsArray); //converted array to a list 
            generator.GenerateCylinder(rows, amountPerRow, invertedRowsList, collapsed);
            calculator.CalculateDimensions(rows, amountPerRow, invertedRowsList);
            widthTMP.text = "Width: " + calculator.GetWidth() + " cm";
            heightTMP.text = "Height: " + calculator.GetHeight() + " cm";
        }
    }

    private void ShowErrorMessage(string errorDescription)
    {
        errorPanel.SetActive(true);
        errorText.text = errorDescription;
    }

    public void OnAddInvertedRowsChecked(bool checkedOrUnchecked)
    {
        whereToAddInvertedRow.gameObject.SetActive(checkedOrUnchecked);
        if (checkedOrUnchecked)
        {
            whereToAddInvertedRow.ActivateInputField();
        }
        else
        {
            whereToAddInvertedRow.DeactivateInputField();
            whereToAddInvertedRow.text = "0";
        }
    }

    public void WhichRows() //was for testing
    {
        //int[] userAnswers = Array.ConvertAll<string, int>(whereToAddInvertedRow.text.Split(','), int.Parse);
       /* int[] userAnswers = Array.ConvertAll<string, int>(whereToAddInvertedRow.text.Split(','), int.Parse);
        
        for (int i = 0; i < userAnswers.Length; i++)
        {
            Debug.Log(userAnswers[i]);
        }*/
    }
}
