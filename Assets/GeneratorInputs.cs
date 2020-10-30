using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GeneratorInputs : MonoBehaviour
{
    [SerializeField] private TMP_InputField rowsInput = default;
    [SerializeField] private TMP_InputField amountPerRowInput = default;
    [SerializeField] private Toggle collapsedInput = default;
    [SerializeField] private GameObject errorPanel = default;
    [SerializeField] private GameObject generatorObject;
    [SerializeField] private TMP_InputField whereToAddInvertedRow;
    
    private CircleGenerator generator;
    private TMP_Text errorText;
    private float rows;
    private int amountPerRow;
    private bool collapsed;
    void Awake()
    {
        generator = generatorObject.GetComponent<CircleGenerator>();
        errorText = errorPanel.transform.Find("ErrorMessage").GetComponent<TMP_Text>();
        errorPanel.SetActive(false);
        whereToAddInvertedRow.gameObject.SetActive(false);
        rowsInput.text = "1";
        amountPerRowInput.text = "10";
    }
    public void onSubmit()
    {
        GameObject generatedCylinder = null; 
        // https://answers.unity.com/questions/1151762/check-if-inputfield-is-empty.html
        if (!string.IsNullOrEmpty(rowsInput.text) || !string.IsNullOrEmpty(amountPerRowInput.text))
        {
           rows = float.Parse(rowsInput.text);
           amountPerRow = int.Parse(amountPerRowInput.text);
        }
        else
        {
            ShowErrorMessage("Please provide values for all input fields.");
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
            generator.GenerateCylinder(rows, amountPerRow, 3, collapsed);
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
    }

    public void WhichRows()
    {
        int[] userAnswers = Array.ConvertAll<string, int>(whereToAddInvertedRow.text.Split(','), int.Parse);
        for (int i = 0; i < userAnswers.Length; i++)
        {
            Debug.Log(userAnswers[i]);
        }
    }
}
