using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Customization : MonoBehaviour
{
    private GameObject currentCylinder;
    private IntEvent OnAddRemove;
    private ColorManager colorManager;
    private CircleGenerator generator;
    private int extraPieces;
    private CalculateWidthHeight calculator;
    private List<int> alreadyInvertedRows;
    void Awake()
    {
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
        for (int i = 1; i < generatedRows.Length; i++)
        {
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

    public void InvertRows(string rowsToInvert)
    {
        bool[] invertedInfo = new bool[3];
        GameObject[] generatedRows = GameObject.FindGameObjectsWithTag("Row");
        // for every row on generated Rows that has a number mentioned in rowsToInvert[] I want to turn it around 
        int index = 0;
        if (!rowsToInvert.Contains("0"))
        {
            for (int i = 0; i < generatedRows.Length; i++)
            {
                if (rowsToInvert.Contains((i + 1).ToString())) //only works thanks to LINQ
                {
                    alreadyInvertedRows.Add(i);
                    index++;
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

                    if (index == generatedRows.Length)
                    {
                        // all rows are inverted
                        invertedInfo[2] = true;
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
        }
        else
        {
            // re-invert all rows if the input is 0 
            foreach(int rowIndex in alreadyInvertedRows)
            {
                if (rowIndex == 1)
                {
                    //very first row is inverted
                    invertedInfo[0] = false;
                }

                if (rowIndex == generatedRows.Length)
                {
                    // top most row is inverted
                    invertedInfo[1] = false;
                }

                if (index == generatedRows.Length)
                {
                    // all rows are inverted
                    invertedInfo[2] = false;
                }
                Transform[] oldChildren = generatedRows[rowIndex].GetComponentsInChildren<Transform>();
                 for (int j = 0; j < oldChildren.Length; j++)
                 {
                     // turn the piece to face inwards
                     oldChildren[j].rotation *= Quaternion.Euler(0f, 180f, 0f);
                     //leave the y position as it was 
                     oldChildren[j].position = Vector3.Scale(oldChildren[j].position, new Vector3(8f/13f, 1, 8f/13f));
                 }
            }
            alreadyInvertedRows.Clear();
        }
        calculator.CalculateDimensions(invertedInfo);
    }
}
