using UnityEngine;
using UnityEngine.UI;
public class CylinderRotation : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
    public float rotationSpeed = 100.0f;
    
    private Button resetButton;
    private float rotationX, rotationY = 0;
    private Quaternion startRotation;

    void Start()
    {
        resetButton = GameObject.FindGameObjectWithTag("Reset").GetComponent<Button>();
        resetButton.onClick.AddListener(ResetRotation);
        startRotation = transform.rotation;
    }
    
    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        
        // rotation works with WSAD and Arrow Keys
        rotationX = Input.GetAxis("Vertical") * rotationSpeed;
        rotationY = Input.GetAxis("Horizontal") * rotationSpeed;
        rotationX *= Time.deltaTime;
        rotationY *= Time.deltaTime;
        
        transform.Rotate(rotationX,-1 * rotationY,0, Space.Self);

        // reset rotation with space
        if(Input.GetKeyDown(KeyCode.Return))
        {
            ResetRotation();
        }
    }
    
    private void ResetRotation()
    {
        transform.rotation = startRotation;
    }
}
