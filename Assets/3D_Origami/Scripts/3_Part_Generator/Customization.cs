using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Customization : MonoBehaviour
{
    [SerializeField] private TMP_InputField rowsInput = default;
    private int rows;
    private GameObject currentCylinder;
    private IntEvent OnAddRemove;
    private ColorManager colorManager;
    private CircleGenerator generator;
    private int extraPieces;
    private CalculateWidthHeight calculator;
    public List<int> alreadyInvertedRows;
    void Awake()
    {
        rows =  int.Parse(rowsInput.text);
        colorManager = FindObjectOfType<ColorManager>();
        calculator = FindObjectOfType<CalculateWidthHeight>();
        generator = FindObjectOfType<CircleGenerator>();
        alreadyInvertedRows = new List<int>();

        if (OnAddRemove == null)
        {
            OnAddRemove = new IntEvent();
        }

        OnAddRemove.AddListener(colorManager.InputCallback);
    }
    
    public void CollapseCylinder(bool collapse)
    {
        GameObject[] generatedRows = GameObject.FindGameObjectsWithTag("Row");
        Vector3 pos;
        for (int i = 1; i < generatedRows.Length; i++)
        {
            pos = generatedRows[i].transform.position;
            if (collapse)
            {
                generatedRows[i].transform.position = new Vector3(0, 0, 0);
            }
            else
            {
                generatedRows[i].transform.position = new Vector3(0, i, 0);
            }

        }
    }
    
    //TODO maybe have a "prepare information" function 
    
    public void RowOnTop()
    {
        extraPieces = 0;
        rows++;
        rowsInput.text = rows.ToString();
        // get current Rows
        GameObject[] generatedRows = GameObject.FindGameObjectsWithTag("Row");
        if (generatedRows.Length != 30)
        {
            int topRowIndex = generatedRows.Length;
            GameObject topRow = generatedRows[generatedRows.Length - 1];
            int piecesInTopRow = topRow.transform.childCount;
            bool even = topRowIndex % 2 == 0;
            GameObject row = new GameObject {name = topRowIndex + 1 + ".row"};
            // parent new row to current Cylinder object
            currentCylinder = GameObject.FindGameObjectWithTag("Cylinder");
            row.transform.parent = currentCylinder.transform;
            Debug.Log(topRow.transform.position);
            row.gameObject.tag = "Row";
            
            for (int a = 0; a < piecesInTopRow; a++)
            {
                extraPieces++;

                float angle;
                float angleSection = Mathf.PI * 2f / piecesInTopRow;

                if (even) // even row starts counting at 0 degrees
                {
                    angle = a * angleSection;
                }
                else // odd row starts counting at half of the angleSection
                {
                    angle = (a * angleSection) + (angleSection / 2f);
                }

                GameObject piece = generator.AssemblePieces(angle, 0.08f, piecesInTopRow,
                    new Vector3(0, 0.5f * topRowIndex, 0), 0);
                // naming every instantiated piece according to its respective row 
                piece.name = a + 1 + ".piece in the " + (topRowIndex + 1) + ".row";
                // parenting every piece to its respective row 
                piece.transform.parent = row.transform;
                piece.AddComponent<ColorOrigami>();
            }
            // if the cylinder has been rotated, generate the row with the same rotation 
            row.transform.rotation = currentCylinder.transform.rotation;

            // sending the change in piece amount to ColorManager Script
            OnAddRemove.Invoke(extraPieces);
            calculator.CalculateDimensions(true);
        }
    }
    
    public void RemoveRow()
    {
        rows--;
        rowsInput.text = rows.ToString();
        GameObject[] generatedRows = GameObject.FindGameObjectsWithTag("Row");
        int topRowIndex = generatedRows.Length -1 ;
        GameObject topRow = generatedRows[topRowIndex];
        int piecesInTopRow = topRow.transform.childCount;
        // allow to only remove pieces until two rows remain 
        if (topRowIndex != 1)
        {
            Destroy(generatedRows[topRowIndex]);
            OnAddRemove.Invoke(- piecesInTopRow);
            calculator.CalculateDimensions(false);
        }
    }

    public void InvertOrRevert(string rowsToInvertOrRevert)
    {
        GameObject[] generatedRows = GameObject.FindGameObjectsWithTag("Row");
        // holds information about the types of inverted rows for the width heig calculation
        // initialized to everything being false
        bool[] invertedInfo = new bool[3];
        // invert
        if (!rowsToInvertOrRevert.Contains("0"))
        {
            invertedInfo[2] = true;
            NewInvertRows(rowsToInvertOrRevert, generatedRows, invertedInfo);
        }
        else
        {
            //revert
            
            // initialize the array to be true everywhere
            for (int i = 0; i < 3; i++) { invertedInfo[i] = true; }
            // gives CalculateHeightWidth Script the Info to revert
            invertedInfo[2] = false;
            RevertRows(generatedRows, invertedInfo);
        }
    }

    private void NewInvertRows(string rowsToInvert, GameObject[] generatedRows, bool[] invertedInfo)
    {
        // invert
        for (int i = 0; i < generatedRows.Length; i++)
        {
            //only works thanks to LINQ
            if (rowsToInvert.Contains((i + 1).ToString()) && !alreadyInvertedRows.Contains(i+1))
            {
                alreadyInvertedRows.Add(i + 1);
                Transform[] children = generatedRows[i].GetComponentsInChildren<Transform>();
                if (i + 1 == 1)
                {
                    //very first row is inverted
                    invertedInfo[0] = true;
                }
                    
                if (i + 1 == generatedRows.Length)
                {
                    // top most row is inverted
                    invertedInfo[1] = true;
                }
                    
                for (int j = 0; j < children.Length; j++)
                {
                    // turn the piece to face inwards
                    children[j].rotation *= Quaternion.Euler(0f, 180f, 0f);
                    //leave the y position as it was 
                    children[j].position = Vector3.Scale(children[j].position, new Vector3(13f / 8f, 1, 13f / 8f));

                }
            }
        }
        calculator.CalculateDimensions(invertedInfo);
    }

    private void RevertRows(GameObject[] generatedRows, bool[] revertedInfo)
    {
        // re-invert all rows if the input is 0 
        // https://stackoverflow.com/questions/604831/collection-was-modified-enumeration-operation-may-not-execute
            foreach(int rowIndex in alreadyInvertedRows.ToList())
            {
                Debug.Log("rowIndex " + rowIndex);
                alreadyInvertedRows.Remove(rowIndex);
                // revert 
                if (rowIndex == 1)
                {
                    //very first row is NOT inverted anymore
                    revertedInfo[0] = false;
                    Debug.Log("//very first row is not inverted anymore");
                }
                if (rowIndex == generatedRows.Length)
                {
                    // top most row is NOT inverted
                    revertedInfo[1] = false;
                    Debug.Log("// top most row is NOT inverted");
                }
                Transform[] oldChildren = generatedRows[rowIndex - 1].GetComponentsInChildren<Transform>();
                for (int j = 0; j < oldChildren.Length; j++)
                {
                    // turn the piece to face inwards
                    oldChildren[j].rotation *= Quaternion.Euler(0f, 180f, 0f);
                    //leave the y position as it was 
                    oldChildren[j].position = Vector3.Scale(oldChildren[j].position, new Vector3(8f/13f, 1, 8f/13f));
                }
            }
            calculator.CalculateDimensions(revertedInfo);
            
        Debug.Log("alreadyInvertedRows.Count " + alreadyInvertedRows.Count);
        
    }
}
