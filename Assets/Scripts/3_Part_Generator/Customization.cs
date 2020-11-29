using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customization : MonoBehaviour
{
    private GameObject currentCylinder;
    public IntEvent OnAddRemove;
    private ColorManager colorManager;
    private CircleGenerator generator;
    private int extraPieces;
    void Awake()
    {
        colorManager = FindObjectOfType<ColorManager>();
        generator = FindObjectOfType<CircleGenerator>();
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
        //TODO somehow make this not be a repetition of GenerateCylinder()
        //TODO communicate with Height/Width Script
        
        // get current Rows
        GameObject[] generatedRows = GameObject.FindGameObjectsWithTag("Row");
        
        int topRowIndex = generatedRows.Length;
        GameObject topRow = generatedRows[generatedRows.Length - 1];
        int piecesInTopRow = topRow.transform.childCount;
        bool even = topRowIndex % 2 == 0;
        GameObject row = new GameObject {name = topRowIndex + 1 + ".row"};
        // parent new row to current Cylinder object
        currentCylinder = GameObject.FindGameObjectWithTag("Cylinder");
        row.transform.parent = currentCylinder.transform;
        row.gameObject.tag= "Row";
       
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
            GameObject piece = generator.AssemblePieces(angle, 0.08f, piecesInTopRow, new Vector3(0 , 0.5f * topRowIndex, 0), 0);
            // naming every instantiated piece according to its respective row 
            piece.name = a + 1 + ".piece in the " + (topRowIndex + 1) + ".row";
            // parenting every piece to its respective row 
            piece.transform.parent = row.transform;
            piece.AddComponent<ColorOrigami>();
        }
        // sending the change in piece amount to ColorManager Script
        OnAddRemove.Invoke(extraPieces);
    }

    public void RemoveRow()
    {
        GameObject[] generatedRows = GameObject.FindGameObjectsWithTag("Row");
        int topRowIndex = generatedRows.Length -1 ;
        GameObject topRow = generatedRows[generatedRows.Length - 1];
        int piecesInTopRow = topRow.transform.childCount;
        Destroy(generatedRows[topRowIndex]);
        OnAddRemove.Invoke(- piecesInTopRow);
    }


    public void InvertRows(string rowsToInvert)
    {
        GameObject[] generatedRows = GameObject.FindGameObjectsWithTag("Row");
        // for every row on generated Rows that has a number mentioned in rowsTOInvert[] I want to turn it around 

        for (int i = 0; i < generatedRows.Length; i++)
        {
            if(rowsToInvert.Contains((i+1).ToString())) //only works thanks to LINQ
            {
                Transform[] children = generatedRows[i].GetComponentsInChildren<Transform>();
                for (int j = 0; j < children.Length; j++)
                {
                    // TODO invert back when 0 
                    // TODO  has to interact with height width script
                    // turn the piece to face inwards
                    children[j].rotation *= Quaternion.Euler(0f, 180f, 0f); 
                    //leave the y position as it was 
                    children[j].position = Vector3.Scale(children[j].position, new Vector3(13f / 8f, 1, 13f / 8f));

                }
            }
        }
    }
}
