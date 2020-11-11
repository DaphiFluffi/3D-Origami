using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CircleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject pieceModel = default;
    //[SerializeField] private GameObject decreasedPieceModel = default;
    /*[SerializeField] private float rows = 4f;
    [SerializeField] private int amountPerRow = 5;
    [SerializeField] private bool collapsed = false;
    [SerializeField] private int invertedRow = default;*/

    private GameObject generatedCylinder;
    private bool isCreated;
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
        if (!isCreated) // so that only one cylinder is created
        {
            Vector3 center = new Vector3(0, 0, 0); 
         
            float angle;
            // empirically, with this distance the pieces look like they were stacked on top of each other
            float distance = 0.5f; 
            
            // parent cylinder object
            generatedCylinder = new GameObject {name = "cylinder"};
            generatedCylinder.AddComponent<CylinderRotation>();
            
            for (int r = 0; r < rows.Length; r++)
            {
                center.y = distance * r;
                //parent row object
                GameObject row = new GameObject {name = r + 1 + ".row"};
                row.transform.parent = generatedCylinder.transform;
                // tag row object
                row.gameObject.tag= "Row";
                
                if (rows[r] == 2) //decreased row 
                {
                    /*
                     * When you make a "decreased row" that means that you are putting the two pockets every 3D Origami piece has
                     * over three and not two tips as you normally would. That leads to the rows above the decreased row to have
                     * less pieces per row.
                     * To calculate the decreased amount you first determine the amount of tips in the previous rows.
                     * Since every piece has two tips we achieve that with 2*amountPerRow. The amount of tips has to be divided
                     * by 3 because a piece in a decreased row goes over 3 tips.
                     */
                    
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
                    // TODO increased heißt schmaler machen
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
