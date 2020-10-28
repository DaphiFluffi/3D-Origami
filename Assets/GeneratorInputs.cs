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

    private TMP_Text errorText;
    private float rows;
    private int amountPerRow;
    private bool collapsed;
    void Awake()
    {
        errorText = errorPanel.transform.Find("ErrorMessage").GetComponent<TMP_Text>();
        errorPanel.SetActive(false);
        rowsInput.text = "1";
        amountPerRowInput.text = "10";
    }
    public void onSubmit()
    {
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

        collapsed = collapsedInput;
        
        if (rows < 1 || rows > 30)
        {
            ShowErrorMessage("Rows have a minimum value of 1 and a maximum of 30.");
            Debug.Log("rows");
        }else if (amountPerRow < 10 || amountPerRow > 50)
        {
            ShowErrorMessage("At least 10 and at most 50 pieces per row are required.");
            
            Debug.Log("pieces" + amountPerRow);
        }
        else
        {
            errorPanel.SetActive(false);
        }
    }

    void ShowErrorMessage(string errorDescription)
    {
        errorPanel.SetActive(true);
        errorText.text = errorDescription;
    }
    
    public float GetRows()
    {
        return rows;
    }
    
    public int GetAmountPerRow()
    {
        return amountPerRow;
    }
    
    public bool getCollapsed()
    {
        return collapsed;
    }

}
