﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
[SerializeField] private GameObject pieceModel = default;
    //[SerializeField] private GameObject decreasedPieceModel = default;
    /*[SerializeField] private float rows = 4f;
    [SerializeField] private int amountPerRow = 5;
    [SerializeField] private bool collapsed = false;
    [SerializeField] private int invertedRow = default;*/

    private GameObject generatedCylinder;
    private bool isCreated;

    private int totalPieces;

    public int GetTotalPieces()
    {
        return totalPieces;
    }
    /*private void Start()
    {
        GenerateCylinder(rows, prefabToInstantiate, amountPerRow, invertedRow);
    }*/

    /// <summary> 
    ///    Spawns a cylinder with an equal amount of pieces per row
    ///    while the amount of rows is up to the user.
    /// </summary>
    /// <param name="pieceModel">The object it will be instantiated</param>
    /// <param name="rows">How many rows should be generated</param>
    /// <param name="amountPerRow">The number of objects per row</param>
    /// <param name="radius">
    ///     The margin from center, if your center is at (1,1,1) and your radius is 3 
    ///     your final position can be (4,1,1) for example </param>
    /// <param name="distance">Distance between rows</param>
   
    
    public void GenerateCylinder(int[] rows, int amountPerRow)
    {
        // replace the old generated Cylinder once a new one is requested to be generated
        Destroy(generatedCylinder);
        isCreated = false;
        totalPieces = 0;
        if (!isCreated) // so that only one cylinder is created
        {
            Vector3 center = new Vector3(0, 0, 0); 
         
            float angle;
            // empirically, with this distance the pieces look like they were stacked on top of each other
            float distance = 0.5f; 
            
            // parent cylinder object
            generatedCylinder = new GameObject {name = "cylinder"};
            generatedCylinder.AddComponent<CylinderRotation>();
            generatedCylinder.gameObject.layer = LayerMask.NameToLayer("All");

            for (int r = 0; r < rows.Length; r++)
            {
                center.y = distance * r;
                //parent row object
                GameObject row = new GameObject {name = r + 1 + ".row"};
                row.transform.parent = generatedCylinder.transform;

                // tag row object
                row.gameObject.tag= "Row";
                row.gameObject.layer = LayerMask.NameToLayer("Row");
                //row.AddComponent<MeshCollider>();
                if (rows[r] == 2) //decreased row 
                {
                    // integer divison automatically takes the first number before comma
                    // only allow rows that are divisible by 3 
                    int decreasedAmount = (2 * amountPerRow) / 3; 
                    /*if (amountPerRow % 3 > 0) // not a multiple of 3 
                    {
                        decreasedAmount += 1;
                    }*/
                    amountPerRow = decreasedAmount;
                }
                if (rows[r] == 3) //increased row 
                {
                    int increasedAmount = (2 * amountPerRow);
                    amountPerRow = increasedAmount;
                }
                
                for (int a = 0; a < amountPerRow; a++)
                {
                    float angleSection = Mathf.PI * 2f / amountPerRow;

                    if (r % 2 == 0) // even row starts counting at 0 degrees
                    {
                        angle = a * angleSection;
                    }
                    else // odd row starts counting at half of the angleSection
                    {
                        angle = (a * angleSection) + (angleSection / 2f);
                    }

                    GameObject piece;
                    
                    if (rows[r] == 1) //inverted Row
                    {
                        piece = AssemblePieces(angle, 0.13f, amountPerRow, center, 0);
                        piece.transform.rotation *= Quaternion.Euler(0f, 180f, 0f); //turn the piece to face inwards
                    }
                    else if (rows[r] == 2) //decreased row
                    {
                        // yPosition was added so that the decreased row faces inwards slightly
                        // that way the tips dont overlap with the row on top 
                        piece = AssemblePieces(angle, 0.075f, amountPerRow, center, 0.1f); 
                        // when you put a piece over 3 tips, it looks larger 
                        piece.transform.localScale += new Vector3(0.7f, 0, 0);
                    }
                    else if (rows[r] == 3 ) //increased row
                    {
                        // yPosition was added so that the decreased row faces inwards slightly
                        // that way the tips dont overlap with the row on top 
                        piece = AssemblePieces(angle, 0.035f, amountPerRow, center, 0.1f); 
                        // when you put a piece over 1 tip, it looks smaller
                        piece.transform.localScale -= new Vector3(0.4f, 0, 0);
                        if (r + 1 < rows.Length)
                        {
                            rows[r + 1] = 4;
                        }
                    }
                    else if (rows[r] == 4)
                    {
                        piece = AssemblePieces(angle, 0.035f, amountPerRow, center, 0.1f); 
                        // when you put a piece over 1 tip, it looks smaller
                        piece.transform.localScale -= new Vector3(0.4f, 0, 0);
                        if (r + 1 < rows.Length)
                        {
                            rows[r + 1] = 4;
                        }
                    }
                    else // normal row
                    {
                        piece = AssemblePieces(angle, 0.08f, amountPerRow, center, 0);
                    }
                    
                    // naming every instantiated piece according to its respective row 
                    piece.name = a + 1 + ".piece in the " + (r + 1) + ".row";
                    // parenting every piece to its respective row 
                    piece.transform.parent = row.transform;
                    //piece.transform.GetChild(0).gameObject.AddComponent<ColorOrigami>();
                    piece.AddComponent<ColorOrigami>();
                }
            }
            isCreated = true;
        }
    }

    private GameObject AssemblePieces(float angle, float radius, int amountPerRow, Vector3 center, float yPosition)
    {
        radius = radius * amountPerRow; 
        // How to instantiate game objects around a point https://answers.unity.com/questions/1068513/place-8-objects-around-a-target-gameobject.html
        // How to make game objects look towards a point https://forum.unity.com/threads/instantiate-prefab-towards-object.134213/
        Vector3 spawnPosition = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        spawnPosition.y += yPosition;
        // so that the pieces look towards the center 
        GameObject piece = Instantiate(pieceModel, spawnPosition, Quaternion.LookRotation((center - spawnPosition) + new Vector3(0,0.1f, 0)));
        piece.gameObject.layer = LayerMask.NameToLayer("Piece");
        totalPieces++;
        return piece;
    }
    
    //TODO put this outside the generator -> maybe a customizationScript
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

    public void RowOnTop()
    {
        // herausfinden, wie viele Reihen es bisher gab
        // eine Reihe mit der teilanzahl der obersten Reihe hinzufügen 
        // je nach dem ob es eine gerade oder ungerade Reihe ist, verdrehen 
        // TODO an der richtigen höhe spawnen!!!
        GameObject[] generatedRows = GameObject.FindGameObjectsWithTag("Row");
        GameObject topRow = generatedRows[generatedRows.Length - 1];
        int topRowIndex = generatedRows.Length;
        bool even;
        even = topRowIndex % 2 == 0;
        int piecesInTopRow = topRow.transform.childCount;
        float angleSection = Mathf.PI * 2f / piecesInTopRow;
        GameObject row = new GameObject {name = topRowIndex + 1 + ".row"};
        //TODO could be problematic
        row.transform.parent = generatedCylinder.transform;
        row.gameObject.tag= "Row";

        
        for (int a = 0; a < piecesInTopRow; a++)
        {
            float angle;
            if (even) // even row starts counting at 0 degrees
            {
                angle = a * angleSection;
            }
            else // odd row starts counting at half of the angleSection
            {
                angle = (a * angleSection) + (angleSection / 2f);
            }

            GameObject piece = AssemblePieces(angle, 0.1f, piecesInTopRow, new Vector3(0, 0, 0), 0);
            // naming every instantiated piece according to its respective row 
            piece.name = a + 1 + ".piece in the " + (topRowIndex + 1) + ".row";
            // parenting every piece to its respective row 
            piece.transform.parent = row.transform;
            piece.transform.position = new Vector3(0, 0.5f * (topRowIndex), 0);

            //piece.transform.GetChild(0).gameObject.AddComponent<ColorOrigami>();
            piece.AddComponent<ColorOrigami>();
        }
        
    }

   public void RemoveRow(){}


    /*public void InvertRows(string rowsToInvert)
    {
        GameObject[] generatedRows = GameObject.FindGameObjectsWithTag("Row");
        // for every row on generated Rows that has a number mentioned in rowsTOInvert[] I want to turn it around 

        for (int i = 0; i < generatedRows.Length; i++)
        {
            if(rowsToInvert.Contains((i+1).ToString())) //only works thanks to LINQ
            {
                Debug.Log((i+1));
                Transform[] children = generatedRows[i].GetComponentsInChildren<Transform>();
                for (int j = 0; j < children.Length; j++)
                {
                    // invert back when 0 
                    //change the radius too :( 
                    children[j].rotation *= Quaternion.Euler(0f, 180f, 0f); //turn the piece to face inwards
                }
            }
        }
    }*/
}
