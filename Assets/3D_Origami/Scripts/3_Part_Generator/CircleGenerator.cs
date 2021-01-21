using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject pieceModel = default;
    
    private GameObject generatedCylinder;
    private bool isCreated;

    private int totalPieces;

    public int GetTotalPieces()
    {
        return totalPieces;
    }

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
            //generatedCylinder.AddComponent<MeshCollider>();
            //for export
            generatedCylinder.AddComponent<MeshFilter>();
            generatedCylinder.AddComponent<MeshRenderer>();

            generatedCylinder.gameObject.tag = "Cylinder";

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
                    // integer divison automatically takes the first number before comma
                    // only allow rows that are divisible by 3 
                    int decreasedAmount = (2 * amountPerRow) / 3;
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
                    
                    if (rows[r] == 2) //decreased row
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
                            if (rows[r + 1] == 2)
                            {
                                // for decreased row on top of increased row
                                rows[r + 1] = 2;
                            }
                            else
                            {
                                rows[r + 1] = 4;
                            }
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
                    piece.AddComponent<ColorOrigami>();
                }
            }
            isCreated = true;
        }
    }

    //TODO make this not have to be public anymoreee :(
    public GameObject AssemblePieces(float angle, float radius, int amountPerRow, Vector3 center, float yPosition)
    {
        radius = radius * amountPerRow; 
        // How to instantiate game objects around a point https://answers.unity.com/questions/1068513/place-8-objects-around-a-target-gameobject.html
        Vector3 spawnPosition = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        spawnPosition.y += yPosition;
        // so that the pieces look towards the center 
        // How to make game objects look towards a point https://forum.unity.com/threads/instantiate-prefab-towards-object.134213/#post-908339
        GameObject piece = Instantiate(pieceModel, spawnPosition, Quaternion.LookRotation((center - spawnPosition) + new Vector3(0,0.1f, 0)));
        totalPieces++;
        return piece;
    }
}
