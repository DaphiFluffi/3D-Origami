using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject prefabToInstantiate = default;
    [SerializeField] private float rows = 4f;
    [SerializeField] private int amountPerRow = 5;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float distance= 1f;
  // https://answers.unity.com/questions/1068513/place-8-objects-around-a-target-gameobject.html
    /// <summary>
    ///     Instantiates prefabs around center splited equality. 
    ///     The number of times indicated in <see cref="howMany" /> var is
    ///     the number of parts will be the circle cuted, with taking as a center the location,
    ///     and adding radius from it
    /// </summary>
    /// <param name="prefab">The object it will be intantiated</param>
    /// <param name="location">The center point of the circle</param>
    /// <param name="howMany">The number of parts the circle will be cut</param>
    /// <param name="radius">
    ///     The margin from center, if your center is at (1,1,1) and your radius is 3 
    ///     your final position can be (4,1,1) for example
    /// </param>
    
    private void InstantiateInCircle(GameObject prefab, Vector3 location, int howMany, float radius)
    {
        float angleSection = Mathf.PI * 2f / howMany;
        for (int i = 0; i < howMany; i++)
        {
            float angle = i * angleSection;
            Vector3 newPos = location + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
           // newPos.y = yPosition;
            //https://forum.unity.com/threads/instantiate-prefab-towards-object.134213/
            Instantiate(prefab, newPos, Quaternion.LookRotation (location - newPos)); // so that the pieces look towards the center 
        }
    }

    private void GenerateCylinder(float rows, GameObject prefab, int amountPerRow, float radius, float distance)
    {
        Vector3 center = new Vector3(0, 0, 0);
        float angleSection = Mathf.PI * 2f / amountPerRow;
        float angle = 0f;
        GameObject cylinder = new GameObject {name = "cylinder"};
        for (int r = 0; r < rows; r++){
            center.y = distance * r;
            GameObject parentObject = new GameObject(); //create an 'empty' object
            parentObject.name = r+1 + ".row" ;  //set it's name
            parentObject.transform.parent = cylinder.transform;
            for (int a = 0; a < amountPerRow; a++)
            {
                //print("row: " + r + " amount: " + a);
                if (r % 2 == 0) // even row starts counting at 0 degrees
                {
                    angle = a * angleSection;
                }
                else // odd row start counting at half of the angleSection
                {
                    angle = (a * angleSection) + (angleSection / 2f);
                }

                Vector3 spawnPosition = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                GameObject piece = Instantiate(prefab, spawnPosition, Quaternion.LookRotation (center - spawnPosition)); // so that the pieces look towards the center 
                piece.name = a+1 + ".piece in the " + (r+1) + ".row";
                piece.transform.parent = parentObject.transform;
            }
        }
    }
    public void InstantiateInCircle(GameObject prefab, int howMany, float radius)
    {
        this.InstantiateInCircle(prefab, this.transform.position, howMany, radius); //location is location of the Empty Game Object
    }
    // client EXAMPLE
    private void Start()
    {
       // this.InstantiateInCircle(this.prefabToInstantiate, new Vector3(0,0,0), 12, 2); //ohne die y = 2 position
        // this.InstantiateInCircle(this.prefabToInstantiate, new Vector3(0,1.5f,0), 12, 2);
        GenerateCylinder(rows, prefabToInstantiate, amountPerRow, radius, distance);
    } 
}
