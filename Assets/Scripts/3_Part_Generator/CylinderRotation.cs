using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class CylinderRotation : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
    public float rotationSpeed = 100.0f;
    private float startingPositionX;
    private float startingPositionY;
    
    void Update()
    {
        //MovementPlayer();
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        // TODO restricted vertical rotation
        /*float rotationX = Input.GetAxis("Vertical") * rotationSpeed;
        float rotationY = Input.GetAxis("Horizontal") * rotationSpeed;*/
        float rotationX = CrossPlatformInputManager.GetAxis("Vertical") * rotationSpeed;
        float rotationY = CrossPlatformInputManager.GetAxis("Horizontal") * rotationSpeed;
        rotationX *= Time.deltaTime;
        rotationY *= Time.deltaTime;
        
        transform.RotateAround(new Vector3(0,0,0), new Vector3(rotationX, rotationY,0), rotationSpeed * Time.deltaTime);
        //transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

     
        // reset rotation with space
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ResetRotation();
        }
        
        //Touch Drag Rotation
        // thanks to https://answers.unity.com/questions/1681603/how-to-rotate-an-object-using-touch-controls.html
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startingPositionX = touch.position.x;
                    //startingPositionY = touch.position.y;
                    break;
                case TouchPhase.Moved:
                    if (startingPositionX > touch.position.x)
                    {
                        transform.Rotate(new Vector3(0,1,0), rotationSpeed * Time.deltaTime);
                    }
                    else if (startingPositionX < touch.position.x)
                    {
                        transform.Rotate(new Vector3(0,1,0), -rotationSpeed * Time.deltaTime);
                    }
                   /* if (startingPositionY > touch.position.y)
                    {
                        transform.Rotate(new Vector3(1,0,0), -rotationSpeed * Time.deltaTime);
                    }
                    else if (startingPositionY < touch.position.y)
                    {
                        transform.Rotate(new Vector3(1,0,0), rotationSpeed * Time.deltaTime);
                    }*/
                    break;
                case TouchPhase.Ended:
                    Debug.Log("Touch Phase Ended.");
                    break;
            }
        }
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }
}
