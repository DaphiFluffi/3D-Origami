using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderRotation : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
    public float rotationSpeed = 100.0f;
    // TODO restricted vertical rotation
    void Update()
    { 
        // Get the horizontal axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float rotationX = Input.GetAxis("Vertical") * rotationSpeed;
        float rotationY = Input.GetAxis("Horizontal") * rotationSpeed;
        rotationY *= Time.deltaTime;
        rotationX *= Time.deltaTime;
        // Rotate around new center at x = -4
        transform.RotateAround(new Vector3(0,0,0), new Vector3(rotationX, rotationY,0), rotationSpeed * Time.deltaTime);
        // reset rotation
        if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
