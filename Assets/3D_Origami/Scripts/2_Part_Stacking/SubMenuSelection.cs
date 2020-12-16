using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenuSelection : MonoBehaviour
{
    private string currentTutorial;
    public void WhichTutorial(string tutorialName)
    {
        currentTutorial = tutorialName;
        Debug.Log(currentTutorial);
    }
}
