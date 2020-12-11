using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PanCamera : MonoBehaviour
{
    //https://www.youtube.com/watch?v=4_HUlAFlxwU
    private Vector3 touchStart;
    public Camera cam;
    public float groundZ = 0;
    private Vector3 camStartPos;
    private int fingerID = -1;

    void Awake()
    {
        camStartPos = Camera.main.transform.position;
        #if !UNITY_EDITOR && !_UNITY_WEBGL
             fingerID = 0;
        #endif
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = GetWorldPosition(groundZ);
        }

        if (Input.GetMouseButton(0))
        {
            //https://answers.unity.com/questions/822273/how-to-prevent-raycast-when-clicking-46-ui.html?childToView=862598#answer-862598
            // prevent raycasts when hovering over UI elements so that using the UI does not pan camera
            //https://answers.unity.com/questions/988257/eventsystemispointerovergameobject-for-mobile-devi.html
            // fingerID is so it also works with touch input
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(fingerID)) 
            {
                Vector3 direction = touchStart - GetWorldPosition(groundZ);
                cam.transform.position += direction;
                cam.transform.position = new Vector3(camStartPos.x, cam.transform.position.y, cam.transform.position.z); 
            }
        }
    }

    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0,0, z));
        ground.Raycast(mousePos, out var distance);
        return mousePos.GetPoint(distance);
    }
}