using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationControlls : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
    public float rotationSpeed = 100.0f;

    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float rotationX = Input.GetAxis("Vertical") * rotationSpeed;
        float rotationZ = Input.GetAxis("Horizontal") * rotationSpeed;
        
        rotationX *= Time.deltaTime;
        rotationZ *= Time.deltaTime;
        
        // Rotate around new center at x = -4
        transform.RotateAround(new Vector3(0,0.5f,0), new Vector3(rotationX,0, rotationZ), rotationSpeed * Time.deltaTime);
    }
}
