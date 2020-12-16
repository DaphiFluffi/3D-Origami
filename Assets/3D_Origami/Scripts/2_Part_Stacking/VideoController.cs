using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private string currentTutorial;
    private string stepsInCurrentTutorial;
    [SerializeField] private GameObject VideoCanvas;
    [SerializeField] private GameObject SelectionCanvas;
    [SerializeField] private VideoPlayer videoPlayer = default;
    
    // TODO may have to go before the change scene main menu controller call
    public void WhichTutorial(string tutorialName)
    {
        currentTutorial = tutorialName;
        if (SelectionCanvas.activeInHierarchy)
        {
            SelectionCanvas.SetActive(false);
        }

        if (!VideoCanvas.activeInHierarchy)
        {
            VideoCanvas.SetActive(true);
            videoPlayer.url = "file://C:/Users/mcflu/Documents/Daphna/HTW Berlin - Internationale Medieninformatik/5. Semester HTW/Bachelorarbeit/Videoschnitt_Bachelorarbeit/"+currentTutorial+"/1_"+ currentTutorial+".MP4";
            Debug.Log(videoPlayer.url);
        }
        
    }
    
   
}
