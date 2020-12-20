using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
public class CylinderRotation : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
    public float rotationSpeed = 100.0f;
    private float startingPositionX;
    private float startingPositionY;
    private Button resetButton;
    private float rotationX, rotationY, GUIrotatationX, GUIrotatationY = 0;
    
    private void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }
    
    void Start()
    {
        resetButton = GameObject.FindGameObjectWithTag("Reset").GetComponent<Button>();
        resetButton.onClick.AddListener(ResetRotation);
    }
    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        // TODO restricted vertical rotation
        
        // rotation works with WSAD and Arrow Keys
        rotationX = Input.GetAxis("Vertical") * rotationSpeed;
        rotationY = Input.GetAxis("Horizontal") * rotationSpeed;

        //https://www.youtube.com/watch?v=XFbNTeW1d_8
        // rotation with on Screen keys
        GUIrotatationX = CrossPlatformInputManager.GetAxis("Vertical") * rotationSpeed;
        GUIrotatationY = CrossPlatformInputManager.GetAxis("Horizontal") * rotationSpeed;
        
        rotationX *= Time.deltaTime;
        rotationY *= Time.deltaTime;
        GUIrotatationX *= Time.deltaTime;
        GUIrotatationY *= Time.deltaTime;

        //Generator Scene
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            transform.Rotate(rotationX, rotationY, 0, Space.Self);
            transform.Rotate(GUIrotatationX, GUIrotatationY, 0, Space.Self);
        }
        
        // How to fold Pieces Scene
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            transform.Rotate(rotationX,0, rotationY, Space.Self);
            transform.Rotate(GUIrotatationX,0, GUIrotatationY, Space.Self);
        }
        
        // reset rotation with space
        if(Input.GetKeyDown(KeyCode.Return))
        {
            ResetRotation();
        }
    }
}
