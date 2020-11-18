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
        // TODO restricted vertical rotation
        float rotationX = Input.GetAxis("Vertical") * rotationSpeed;
        float rotationY = Input.GetAxis("Horizontal") * rotationSpeed;
        rotationX *= Time.deltaTime;
        rotationY *= Time.deltaTime;
        
        transform.RotateAround(new Vector3(0,0,0), new Vector3(rotationX, rotationY,0), rotationSpeed * Time.deltaTime);
        
        // reset rotation with space
        if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
