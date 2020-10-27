using UnityEngine;
using UnityEngine.UI;

public class CircleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject prefabToInstantiate = default;
    [SerializeField] private float rows = 4f;
    [SerializeField] private int amountPerRow = 5;
    [SerializeField] private bool collapsed = false;
    [SerializeField] private int invertedRow  ; 
    
   
    private void Start()
    {
        GenerateCylinder(rows, prefabToInstantiate, amountPerRow, invertedRow);
    } 
    
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
    private void GenerateCylinder(float howManyRows, GameObject prefab, int amountPerRow, int invertedRow)
    {
        Vector3 center = new Vector3(0, 0, 0);
        float radius, angle;
        float angleSection = Mathf.PI * 2f / amountPerRow;
        float distance = 1.5f; // determined empirically
        if (collapsed) { distance = 0.5f;}
        
        // parent cylinder object
        GameObject cylinder = new GameObject {name = "cylinder"};
        cylinder.AddComponent<RotationControlls>();
        
        for (int r = 0; r < howManyRows; r++)
        {
            center.y = distance * r;
            //parent row object
            GameObject row = new GameObject {name = r + 1 + ".row"};
            row.transform.parent = cylinder.transform;
            
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
                
                if (r == invertedRow - 1)
                {
                    radius = 0.13f * amountPerRow; // was determined empirically
                }
                else
                {
                     radius = 0.08f * amountPerRow; // 0.08f was determined empirically
                }
                // How to instantiate game objects around a point https://answers.unity.com/questions/1068513/place-8-objects-around-a-target-gameobject.html
                // How to make game objects look towards a point https://forum.unity.com/threads/instantiate-prefab-towards-object.134213/
                Vector3 spawnPosition = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                // so that the pieces look towards the center 
                GameObject piece = Instantiate(prefab, spawnPosition, Quaternion.LookRotation (center - spawnPosition));

                if (r == invertedRow - 1)
                {
                    piece.transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
                }
                
                // naming every instantiated piece according to its respective row 
                piece.name = a+1 + ".piece in the " + (r+1) + ".row";
                // parenting every piece to its respective row 
                piece.transform.parent = row.transform;
                

            }
        }
    }
}
