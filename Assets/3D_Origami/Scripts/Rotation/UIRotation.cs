using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIRotation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool moveLeft, moveRight, moveUp, moveDown;
    private string buttonType;
    private GameObject objectToRotate;
    public float rotationSpeed = 100.0f;

    void Update()
    {
        if (moveLeft)
        {
            objectToRotate.transform.Rotate(new Vector3(0,1 * rotationSpeed * Time.deltaTime,0));
        }
        if (moveRight)
        {
            objectToRotate.transform.Rotate(new Vector3(0,-1 * rotationSpeed * Time.deltaTime,0));
        }
     
        if (moveUp)
        {
            objectToRotate.transform.Rotate(new Vector3(1 * rotationSpeed * Time.deltaTime,0,0));
        }
        if (moveDown)
        {
            objectToRotate.transform.Rotate(new Vector3(-1 * rotationSpeed * Time.deltaTime,0,0));
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GameObject.FindGameObjectWithTag("ToRotate"))
        {
            objectToRotate = GameObject.FindGameObjectWithTag("Cylinder");
        }
        else
        {
            objectToRotate = GameObject.FindGameObjectWithTag("ToRotate");
        }
        switch (gameObject.name)
        {
            case "UpArrow":
                moveUp = true;
                break;
            case "DownArrow":
                moveDown = true;
                break;
            case "LeftArrow":
                moveLeft = true;
                break;
            case "RightArrow":
                moveRight = true;
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (gameObject.name)
        {
            case "UpArrow":
                moveUp = false;
                break;
            case "DownArrow":
                moveDown = false;
                break;
            case "LeftArrow":
                moveLeft = false;
                break;
            case "RightArrow":
                moveRight = false;
                break;
        }
    }
}
