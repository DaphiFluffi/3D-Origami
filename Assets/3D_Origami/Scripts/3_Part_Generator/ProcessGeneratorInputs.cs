using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Events;

[System.Serializable] public class IntEvent : UnityEvent<int> { }
//responsible for checking the input and preparing it for the generator
public class ProcessGeneratorInputs : MonoBehaviour
{
    [SerializeField] private TMP_InputField rowsInput = default;
    [SerializeField] private TMP_InputField amountPerRowInput = default;
    [SerializeField] private TMP_InputField whereToAddDecreasedRow = default;
    [SerializeField] private TMP_InputField whereToAddIncreasedRow = default;
    [SerializeField] private TMP_InputField whereToAddInvertedRow = default;
    [SerializeField] private Toggle collapsed = default;
   
    private IntEvent OnNewCylinder;
    private ColorManager colorManager;
    
    private CircleGenerator generator;
    private CalculateWidthHeight calculator;
    private Customization customization;
    private int howManyRows;
    private int[] rowsInfo;
    private int amountPerRow;
    
    
    void Awake()
    {
        generator = FindObjectOfType<CircleGenerator>();
        calculator = FindObjectOfType<CalculateWidthHeight>();
        customization = FindObjectOfType<Customization>();
        customization.gameObject.SetActive(false);
        // generates the most basic, but stable, base 3D Origami model
        // prevents false input exception
        rowsInput.text = "3";
        amountPerRowInput.text = "9";
        whereToAddDecreasedRow.text = "0";
        whereToAddIncreasedRow.text = "0";
        
        colorManager = FindObjectOfType<ColorManager>();

        
        if (OnNewCylinder == null)
        {
            OnNewCylinder = new IntEvent();
        }

        OnNewCylinder.AddListener(colorManager.InputCallback);
    }
    
    
    // can only be accessed if all Validations were passed
    public void OnSubmit()
    {
        collapsed.isOn = true;
        whereToAddInvertedRow.text = "0";
        howManyRows = int.Parse(rowsInput.text);
        amountPerRow = int.Parse(amountPerRowInput.text);
        
        rowsInfo = new int[howManyRows];
        
        //TODO I should probably sort the Array
        int[] decreased = Array.ConvertAll<string, int>(whereToAddDecreasedRow.text.Split(','), int.Parse);
        int[] increased = Array.ConvertAll<string, int>(whereToAddIncreasedRow.text.Split(','), int.Parse);

        // in case no special rows are requested
        if (decreased.Contains(0) && increased.Contains(0)) 
        {
            Array.Clear(rowsInfo, 0, rowsInfo.Length);
        }
        else
        {
            for (int i = 0; i < rowsInfo.Length; i++)
            {
                if (decreased.Contains(i + 1))
                {
                    // 2 = decreased
                    rowsInfo[i] = 2;
                }
                else if (increased.Contains(i + 1))
                {
                    // 2 = decreased
                    rowsInfo[i] = 3;
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
        calculator.CalculateDimensions(rowsInfo.Length - 1, amountPerRow);
        // customization is disacled until Generate Button is pressed 
        customization.gameObject.SetActive(true);
        //sends totalPieces to ColorManager after Cylinder was generated
        OnNewCylinder.Invoke(generator.GetTotalPieces());
    }
}
