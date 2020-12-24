using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoSelection : MonoBehaviour
{
    private string currentTutorial;
    private string stepsInCurrentTutorial;

    [SerializeField] private TextMeshProUGUI tutorialIndicator = default;
    // --- Video ---
    [SerializeField] private GameObject VideoCanvas = default;
    [SerializeField] private GameObject SelectionCanvas = default;
    [SerializeField] private VideoPlayer videoPlayer = default;
    private string videoUrlTemplate;
    private int clipIndexInCurrentTutorial;
    
    // --- Instructions ---
    [SerializeField] private TextMeshProUGUI instructionsText = default;
    [SerializeField] private string[] instructions = default;

    // ---Pausing ---
    private bool paused;
    [SerializeField] private Button pauseButton = default;
    [SerializeField] private Sprite Unpause = default;
    [SerializeField] private Sprite Pause = default;
    
    // ---Progress --
    [SerializeField] private Slider progressBar = default;

    private void Start()
    {
        videoUrlTemplate =
            "file://C:/Users/mcflu/Desktop/Videoschnitt_Bachelorarbeit/(currentTutorial)/(videoIndex)_(currentTutorial).MP4";
        VideoCanvas.SetActive(false);
        progressBar.value = 1; // indicate that we are at step 1

        paused = true; // if it is set as true in the beginning, the PauseAnimation() method flips it to false
        PauseOrUnpauseClip();
    }
    
    private void ChangeVideo(string clipIndex)
    {
        videoUrlTemplate = videoUrlTemplate.Replace("(videoIndex)", clipIndex);
        videoPlayer.url = videoUrlTemplate;
        //only change the first occurente of the clipNumber otherwise e.g. in MP4 the 4 will be repaced 
        // https://stackoverflow.com/questions/8809354/replace-first-occurrence-of-pattern-in-a-string
        var regex = new Regex(Regex.Escape(clipIndex.ToString()));
        videoUrlTemplate = regex.Replace(videoUrlTemplate, "(videoIndex)", 1);

    }

    public void BackToSelection()
    {
        SelectionCanvas.SetActive(true);
        //relaod scene
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
        VideoCanvas.SetActive(false);
    }
    
    // TODO may have to go before the change scene main menu controller call
    public void WhichTutorial(string tutorialName)
    {
      
        if (SelectionCanvas.activeInHierarchy)
        {
            SelectionCanvas.SetActive(false);
        }

        if (!VideoCanvas.activeInHierarchy)
        {
            VideoCanvas.SetActive(true);
            //setting all values depending on tutorial
            clipIndexInCurrentTutorial = 1;
            currentTutorial = tutorialName;
            tutorialIndicator.text = currentTutorial;
            int howManyClips = ClipAmountInTutorial();
            progressBar.maxValue = howManyClips;
            instructions = SetOfInstructions();
            //TODO which Instructions to show
            instructionsText.text = instructions[0];

           // videoPlayer.url = "file://C:/Users/mcflu/Documents/Daphna/HTW Berlin - Internationale Medieninformatik/5. Semester HTW/Bachelorarbeit/Videoschnitt_Bachelorarbeit/"+currentTutorial+"/(videoIndex)_"+ currentTutorial+".MP4";
            videoUrlTemplate = videoUrlTemplate.Replace("(currentTutorial)", currentTutorial);
            ChangeVideo("1");
        }
    }
    
    private int ClipAmountInTutorial()
    {
        int clipAmount = 0;

        switch (currentTutorial)
        {
            case "Base":
                clipAmount = 3;
                break;
            case "Bottom":
                clipAmount = 3;
                break;
            case "Decreased":
                clipAmount = 7;
                break;
            case "Increased":
                clipAmount = 5;
                break;
            case "Inverted":
                clipAmount = 5;
                break;
            case "Normal":
                clipAmount = 2;
                break;
        }

        return clipAmount;
    }

    private string[] SetOfInstructions()
    {
        string[] instructions = new string[7];

        switch (currentTutorial)
        {
            case "Base":
                instructions[0] = "1. Take two of your stacks and connect them as shown.";
                instructions[1] = "2. Continue all the way around.";
                instructions[2] = "3. Squeeze the finished product into a cup shape.";
                break;
            case "Bottom":
                instructions[0] = "1. Flip your model on its side.";
                instructions[1] = "2. Turn your piece around and attach and put \n one tip into one pocket of one piece.";
                instructions[2] = "3. This technique can be used in e.g. this lemon design.";
                break;
            case "Decreased":
                instructions[0] = "1. Put one piece over 3 tips.";
                instructions[1] = "2. Continue putting one piece over 3 tips all the way around.";
                instructions[2] = "3. If your row has an amount of tips that is not a multiple of 3, \n  you will have to improvise.";
                instructions[3] = "4. Be cautious: Decreased rows make your model less stable.";
                instructions[4] = "5. For that reason, consider gluing down decreased rows";
                instructions[5] = "6. To improve stability, put at least one normal row on top. \n Consider gluing it too.";
                instructions[6] = "7. This technique is useful to finish your model with a dome.";
                break;
            case "Increased":
                instructions[0] = "1. Put one pocket of your piece over 1 tip.";
                instructions[1] = "2. To make your model more stable, glue down incresed rows.";
                instructions[2] = "3. Continue like this all the way around. \n At the end, the amount of pieces has doubled.";
                instructions[3] = "4. Putting on a normal row above also increases stability.";
                instructions[4] = "5. This technique is used to create curved objects.";
                break;
            case "Inverted":
                instructions[0] = "1. Turn every piece to face inwards \n and put one piece over two tips.";
                instructions[1] = "2. Continue all the way around";
                instructions[2] = "3. Notice, that when applying an new row above it, \n the inverted row is covered completely.";
                instructions[3] = "4. Continue applying a normal row on top.";
                instructions[4] = "5. This technique is used to visibly separate two \n parts of a model, e.g. head and body.";
                break;
            case "Normal":
                instructions[0] = "1. Put one pocket of your piece over 1 tip of one piece \n in the row below. Put the second pocket \n over 1 tip of the piece next to the first one.";
                instructions[1] = "2. Continue all the way around to complete a normal row.";
                break;
        }

        return instructions;
    }

    
    public void NextClip()
    {
        if (clipIndexInCurrentTutorial < ClipAmountInTutorial())
        {
            clipIndexInCurrentTutorial += 1;
            ChangeVideo(clipIndexInCurrentTutorial.ToString());
            progressBar.value = clipIndexInCurrentTutorial;
            instructionsText.text = instructions[clipIndexInCurrentTutorial - 1];
        }
    }

    public void PreviousClip()
    {
        if (clipIndexInCurrentTutorial > 1)
        {
            clipIndexInCurrentTutorial -= 1;
            ChangeVideo(clipIndexInCurrentTutorial.ToString());
            progressBar.value = clipIndexInCurrentTutorial;
            instructionsText.text = instructions[clipIndexInCurrentTutorial - 1];
        }
    }
    
    public void PauseOrUnpauseClip()
    {
        if (paused) 
        {
            UnpauseClip();
        }
        else 
        {
            PauseClip();
        }
    }
    
    private void PauseClip()
    {
        videoPlayer.Pause();
        pauseButton.image.overrideSprite = Unpause;//"Unpause";
        paused = true;
    }
    
    private void UnpauseClip()
    {
        videoPlayer.Play();
        pauseButton.image.overrideSprite = Pause; //"Pause";
        paused = false;
    }

    public void SpeedSettings(float sliderSpeed)
    {
        if (videoPlayer.canSetPlaybackSpeed)
        {
            videoPlayer.playbackSpeed = sliderSpeed;
        }
    }
}
