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
    void Awake()
    {
        camStartPos = Camera.main.transform.position;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = GetWorldPosition(groundZ);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - GetWorldPosition(groundZ);
            cam.transform.position += direction;
            cam.transform.position = new Vector3( camStartPos.x, cam.transform.position.y, cam.transform.position.z);
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