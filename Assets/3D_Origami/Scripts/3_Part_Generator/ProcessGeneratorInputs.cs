using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

[System.Serializable] public class IntEvent : UnityEvent<int> { }
//responsible for checking the input and preparing it for the generator
public class ProcessGeneratorInputs : MonoBehaviour
{
    [SerializeField] private TMP_InputField rowsInput = default;
    [SerializeField] private TMP_InputField amountPerRowInput = default;
    [SerializeField] private GameObject origamiObject = default;
    [SerializeField] private TMP_InputField whereToAddInvertedRow = default;
    [SerializeField] private TMP_InputField whereToAddDecreasedRow = default;
    [SerializeField] private TMP_InputField whereToAddIncreasedRow = default;
    [SerializeField] private TMP_Text widthTMP = default;
    [SerializeField] private TMP_Text heightTMP = default;
    [SerializeField] private Toggle collapsed = default;
   
    public IntEvent OnNewCylinder;
    private ColorManager colorManager;

    
    private CircleGenerator generator;
    private CalculateWidthHeight calculator;
    private int howManyRows;
    private int[] rowsInfo;
    private int amountPerRow;
    
    
    void Awake()
    {
        generator = origamiObject.GetComponent<CircleGenerator>();
        calculator = origamiObject.GetComponent<CalculateWidthHeight>();
        // generates the most basic, but stable, base 3D Origami model
        // prevents false input exception
        rowsInput.text = "3";
        amountPerRowInput.text = "9";
        whereToAddInvertedRow.text = "0"; 
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
        howManyRows = int.Parse(rowsInput.text);
        amountPerRow = int.Parse(amountPerRowInput.text);
        
        rowsInfo = new int[howManyRows];
        
        //TODO I should probably sort the Array
        int[] inverted = Array.ConvertAll<string, int>(whereToAddInvertedRow.text.Split(','), int.Parse);
        int[] decreased = Array.ConvertAll<string, int>(whereToAddDecreasedRow.text.Split(','), int.Parse);
        int[] increased = Array.ConvertAll<string, int>(whereToAddIncreasedRow.text.Split(','), int.Parse);

        // in case no special rows are requested
        if (inverted.Contains(0) && decreased.Contains(0) && increased.Contains(0)) 
        {
            Array.Clear(rowsInfo, 0, rowsInfo.Length);
        }
        else
        {
            for (int i = 0; i < rowsInfo.Length; i++)
            {
                if (inverted.Contains(i + 1))
                {
                    // 1 = inverted
                    rowsInfo[i] = 1;
                }
                else if (decreased.Contains(i + 1))
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
       // Generator.Instance.GenerateCylinder(rowsInfo, amountPerRow);
        calculator.CalculateDimensions(rowsInfo, amountPerRow);
        widthTMP.text = "w: " + calculator.GetWidth() + " cm";
        heightTMP.text = "h: " + calculator.GetHeight() + " cm";
        //sends totalPieces to ColorManager after Cylinder was generated
        OnNewCylinder.Invoke(generator.GetTotalPieces());
    }
}
