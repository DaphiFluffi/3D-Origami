using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CircleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject pieceModel = default;
    /*[SerializeField] private float rows = 4f;
    [SerializeField] private int amountPerRow = 5;
    [SerializeField] private bool collapsed = false;
    [SerializeField] private int invertedRow = default;*/

    private GameObject generatedCylinder;
    private bool isCreated;
    GameObject piece;
    /*private void Start()
    {
        GenerateCylinder(rows, prefabToInstantiate, amountPerRow, invertedRow);
    }*/

    /// <summary> 
    ///    Spawns a cylinder with an equal amount of pieces per row
    ///    while the amount of rows is up to the user.
    /// </summary>
    /// <param name="prefab">The object it will be instantiated</param>
    /// <param name="howManyRows">How many rows should be generated</param>
    /// <param name="amountPerRow">The number of objects per row</param>
    /// <param name="radius">
    ///     The margin from center, if your center is at (1,1,1) and your radius is 3 
    ///     your final position can be (4,1,1) for example </param>
    /// <param name="distance">Distance between rows</param>
    
    public void GenerateCylinder(float howManyRows, int amountPerRow, List<int> invertedRows, bool collapsed)
    {
        Destroy(generatedCylinder);
        isCreated = false;
        if (!isCreated) // so that only one cylinder is created
        {
            Vector3 center = new Vector3(-4, 0, 0);
         
            float angle;
            float angleSection = Mathf.PI * 2f / amountPerRow;
            float distance = 1.5f; // determined empirically
            if (collapsed) { distance = 0.5f; }

            // parent cylinder object
            generatedCylinder = new GameObject {name = "cylinder"};
            generatedCylinder.AddComponent<RotationControlls>();

            for (int r = 0; r < howManyRows; r++)
            {
                center.y = distance * r;
                //parent row object
                GameObject row = new GameObject {name = r + 1 + ".row"};
                row.transform.parent = generatedCylinder.transform;

                for (int a = 0; a < amountPerRow; a++)
                {
                    if (r % 2 == 0) // even row starts counting at 0 degrees
                    {
                        angle = a * angleSection;
                    }
                    else // odd row starts counting at half of the angleSection
                    {
                        angle = (a * angleSection) + (angleSection / 2f);
                    }

                    if(invertedRows.Contains(0))
                    {
                        piece = AssemblePieces(angle, 0.08f, amountPerRow, center);
                    }
                    else
                    {
                        if (invertedRows.Contains(r + 1))
                        {
                            piece = AssemblePieces(angle, 0.13f, amountPerRow, center);
                            piece.transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
                        }
                        else
                        {
                            piece = AssemblePieces(angle, 0.08f, amountPerRow, center);
                        }
                    }
                    
                    // naming every instantiated piece according to its respective row 
                    piece.name = a + 1 + ".piece in the " + (r + 1) + ".row";
                    // parenting every piece to its respective row 
                    piece.transform.parent = row.transform;
                    
                    /*GameObject piece;
                    
                    if (r == invertedRow - 1)
                    {
                        piece = AssemblePieces(angle, 0.13f, amountPerRow, center);
                        piece.transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
                        // row.name += "inverted"; //adds the inverted suffix as ofter as we have pieces per inverted row 
                    }
                    else
                    {
                        piece = AssemblePieces(angle, 0.08f, amountPerRow, center);
                    }
                    // naming every instantiated piece according to its respective row 
                    piece.name = a + 1 + ".piece in the " + (r + 1) + ".row";
                    // parenting every piece to its respective row 
                    piece.transform.parent = row.transform;*/
                }
            }
            isCreated = true;
        }
    }

    private GameObject AssemblePieces(float angle, float radius, int amountPerRow, Vector3 center)
    {
        radius = radius * amountPerRow; 
        // How to instantiate game objects around a point https://answers.unity.com/questions/1068513/place-8-objects-around-a-target-gameobject.html
        // How to make game objects look towards a point https://forum.unity.com/threads/instantiate-prefab-towards-object.134213/
        Vector3 spawnPosition = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        // so that the pieces look towards the center 
        GameObject piece = Instantiate(pieceModel, spawnPosition, Quaternion.LookRotation(center - spawnPosition));
        return piece;
    }
    
}
