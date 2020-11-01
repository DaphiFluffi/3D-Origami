using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderRotation : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
    public float rotationSpeed = 100.0f;

    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float rotationX = Input.GetAxis("Vertical") * rotationSpeed;
        float rotationY = Input.GetAxis("Horizontal") * rotationSpeed;
        //float rotationZ = Input.GetAxis("Horizontal") * rotationSpeed;
        
        rotationX *= Time.deltaTime;
        rotationY *= Time.deltaTime;
        //rotationZ *= Time.deltaTime;

        // Rotate around our y-axis
        transform.Rotate(rotationX, rotationY , 0);
    }
}
