using TMPro;
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
    
    // --- Navigation ---
    [SerializeField] private Button nextButton = default; 
    [SerializeField] private Button previousButton = default; 
    [SerializeField] private Button backToLibButton = default; 

    // --- Links/YouTube -- 
    [SerializeField] private YoutubePlayer.YoutubePlayer youtubePlayer = default;
    [SerializeField] private string[] links = default; 
    private void Start()
    {
        //videoUrlTemplate =
            //"file://C:/Users/mcflu/Desktop/Videoschnitt_Bachelorarbeit/(currentTutorial)/(videoIndex)_(currentTutorial).MP4";
            //"Assets/3D_Origami/Video/(currentTutorial)/(videoIndex)_(currentTutorial).MP4";

            //https://github.com/git-lfs/git-lfs/issues/1342
            //trying Github LFS workaround to accces my large video files (Please work)
            //"https://media.githubusercontent.com/media/DaphiFluffi/3D_Origami/beta/Assets/3D_Origami/Video/(currentTutorial)/(videoIndex)_(currentTutorial).MP4";
            //"https://github.com/DaphiFluffi/3D-Origami/blob/gh-pages/Assets/3D_Origami/Video/(currentTutorial)/(videoIndex)_(currentTutorial).MP4?raw=true";
            //"https://github.com/DaphiFluffi/3D-Origami/raw/main//Assets/3D_Origami/Video/(currentTutorial)/(videoIndex)_(currentTutorial).MP4";

        VideoCanvas.SetActive(false);
        progressBar.value = 1; // indicate that we are at step 1

        paused = true; // if it is set as true in the beginning, the PauseAnimation() method flips it to false
        PauseOrUnpauseClip();

        clipIndexInCurrentTutorial = 0;
    }
    
    //TODO
    void Update() // don't show previous button at the first step, don't show next button at the last step 
    {
       /* if (VideoCanvas.activeInHierarchy)
        {
            Debug.Log(clipIndexInCurrentTutorial);
            if (clipIndexInCurrentTutorial == 0)
            {
                previousButton.gameObject.SetActive(false);
                Debug.Log("first clip");
            }
            else if (clipIndexInCurrentTutorial == ClipAmountInTutorial())
            {
                nextButton.gameObject.SetActive(false);
                backToLibButton.gameObject.SetActive(true);
                Debug.Log("last clip");

            }
            else
            {
                previousButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(true);
                backToLibButton.gameObject.SetActive(false);
                Debug.Log("middle");
            }
        }*/
    }
    
   /* private void ChangeVideo(string clipIndex)
    {
        videoUrlTemplate = videoUrlTemplate.Replace("(videoIndex)", clipIndex);
        videoPlayer.url = videoUrlTemplate;
        //only change the first occurente of the clipNumber otherwise e.g. in MP4 the 4 will be repaced 
        // https://stackoverflow.com/questions/8809354/replace-first-occurrence-of-pattern-in-a-string
        var regex = new Regex(Regex.Escape(clipIndex.ToString()));
        videoUrlTemplate = regex.Replace(videoUrlTemplate, "(videoIndex)", 1);

    }*/

    private void ChangeYouTubeVideo(int clipIndex)
    {
        //Debug.Log(links[clipIndex]);
        // prepare new url
        youtubePlayer.youtubeUrl = links[clipIndex];
        youtubePlayer.PlayVideoAsync(youtubePlayer.youtubeUrl);
    }
    
    private void ChangeVimeoVideo(int clipIndex)
    {
        //Debug.Log(links[clipIndex]);
        // prepare new url
        videoPlayer.url = links[clipIndex];
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
            
            // play the first tutorial on button click
            clipIndexInCurrentTutorial = 1;
            // set current Tutorial name
            currentTutorial = tutorialName;
            // put tutorial name into right hand corner
            tutorialIndicator.text = currentTutorial;
            // how many clips are in the current tutorial
            int howManyClips = ClipAmountInTutorial();
            progressBar.maxValue = howManyClips;
           
            instructions = SetOfInstructions();
            //which Instructions to show
            instructionsText.text = instructions[0];

            // --Videos from Desktop
            // videoPlayer.url = "file://C:/Users/mcflu/Documents/Daphna/HTW Berlin - Internationale Medieninformatik/5. Semester HTW/Bachelorarbeit/Videoschnitt_Bachelorarbeit/"+currentTutorial+"/(videoIndex)_"+ currentTutorial+".MP4";
            
            //videoUrlTemplate = videoUrlTemplate.Replace("(currentTutorial)", currentTutorial);
            // ChangeVideo("1");
            
            // -- Videos from YouTube
            // get the links to the videos of the current tutorial 
            //links = YoutubeLinks();
            //ChangeYouTubeVideo(0);
            
            // --Videos from Vimeo
            links = VimeoLinks();
            ChangeVimeoVideo(0);
        }
    }
    
    private int ClipAmountInTutorial()
    {
        int clipAmount = 0;

        switch (currentTutorial)
        {
            case "Base":
                clipAmount = 4;
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

    private string[] YoutubeLinks()
    {
        string[] links = new string[7];
        switch (currentTutorial)
        {
            case "Base":
                links[0] = "https://youtu.be/xHba9yOWJsE";
                links[1] = "https://youtu.be/b2KZcL-LYt0";
                links[2] = "https://youtu.be/b5U3RclJTgU";
                links[3] = "https://youtu.be/H7NwpXZepi4";
            break;
            case "Bottom":
                links[0] = "https://youtu.be/TCkRnTQkXh8";
                links[1] = "https://youtu.be/W9OrpC8sBdU";
                links[2] = "https://youtu.be/dWfi3CpUCkg";
                break;
            case "Decreased":
                links[0] = "https://youtu.be/u9oFJarwdeY";
                links[1] = "https://youtu.be/HCiM2FWo81A";
                links[2] = "https://youtu.be/j8tqbGWzMbU";
                links[3] = "https://youtu.be/dQEFyGnoajU";
                links[4] = "https://youtu.be/IIuQsIv92EA";
                links[5] = "https://youtu.be/Ap0QX9XsMTM";
                links[6] = "https://youtu.be/BTz4PhwCINg";
            break;
            case "Increased": 
                links[0] = "https://youtu.be/WkHtyIo87lY";
               links[1] = "https://youtu.be/ZaXUHYALOF8";
               links[2] = "https://youtu.be/2apg-_DNUMI";
               links[3] = "https://youtu.be/rw7gFay7n3w";
               links[4] = "https://youtu.be/7P4z4B8XyaE";
            break;
            case "Inverted":
                links[0] = "https://youtu.be/k6npNPvaNVM#";
                links[1] = "https://youtu.be/XEn2Yge8Qpg";
                links[2] = "https://youtu.be/UVSnPdpOlgU";
                links[3] = "https://youtu.be/RILQuwRhLa0";
                links[4] = "https://youtu.be/QrJraj_FaFQ";
            break;
            case "Normal":
                links[0] = "https://youtu.be/__1mlkwVXXM";
                links[1] = "https://youtu.be/6ClWJ0TAXu4";
            break;
        }

        return links;
    }
    
    private string[] VimeoLinks()
    {
        string[] links = new string[7];
        switch (currentTutorial)
        {
            case "Base":
                links[0] = "https://player.vimeo.com/external/502102829.hd.mp4?s=c94b9f628d88491f4d89f07cdeba86d32b1c9c6c&profile_id=175";
                links[1] = "https://player.vimeo.com/external/502104322.hd.mp4?s=b0fbcf9d821875ccfb11eae7390509f9e918062b&profile_id=175";
                links[2] = "https://player.vimeo.com/external/502105347.hd.mp4?s=365c96b218303939cf941415d2a397b962df1735&profile_id=175";
                links[3] = "https://player.vimeo.com/external/502109152.hd.mp4?s=2f445bdb0bcfd6c79fb3602f5a7f7c52a19bf1b0&profile_id=175";
                break;
            case "Bottom":
                links[0] = "https://player.vimeo.com/external/502117177.hd.mp4?s=b83f7c5d84f4d66d7856553653db313ae6411a00&profile_id=175";
                links[1] = "https://player.vimeo.com/external/502115905.hd.mp4?s=fd7112393b6abe34420cf0b5d30add079f758a26&profile_id=175";
                links[2] = "https://player.vimeo.com/external/502117004.hd.mp4?s=959af4ff7e2190c4237ef24c375b137ff64268ee&profile_id=175";
                break;
            case "Decreased":
                links[0] = "https://player.vimeo.com/external/502120642.hd.mp4?s=52116964d1f0e564e1c2aa9b1fd489ab858182f0&profile_id=175";
                links[1] = "https://player.vimeo.com/external/502121685.hd.mp4?s=230e9739f561c982ab5662e54cf1dbf0b188de09&profile_id=175";
                links[2] = "https://player.vimeo.com/external/502123887.hd.mp4?s=845c0345a47fa3abc00aaefbd9f493ba89fe8a17&profile_id=175";
                links[3] = "https://player.vimeo.com/external/502124410.hd.mp4?s=dc19047818ffc3f0183d383386f802426d45cf46&profile_id=175";
                links[4] = "https://player.vimeo.com/external/502124731.hd.mp4?s=604bba60a23f59a465e07bcedb34b4820ae280ef&profile_id=175";
                links[5] = "https://player.vimeo.com/external/502117558.hd.mp4?s=9f168ca10f6cf86202dea95767a17917a0f15d66&profile_id=175";
                links[6] = "https://player.vimeo.com/external/502120242.hd.mp4?s=8751f0916980140dabd6c384b2640f8541708bc4&profile_id=175";
                break;
            case "Increased": 
                links[0] = "https://player.vimeo.com/external/502130570.hd.mp4?s=23f631915001e0cb24a98170772a09868822532c&profile_id=175";
                links[1] = "https://player.vimeo.com/external/502131408.hd.mp4?s=ed6866f27e45d0861358b82d3a6c34c67abef1c8&profile_id=175";
                links[2] = "https://player.vimeo.com/external/502132714.hd.mp4?s=149299f8a0f0ed4bc1311a8ebb309462f2d14b8f&profile_id=175";
                links[3] = "https://player.vimeo.com/external/502126339.hd.mp4?s=a4454dd7c4013497ff1d70814d2f1240109ba640&profile_id=175";
                links[4] = "https://player.vimeo.com/external/502130108.hd.mp4?s=22e125309669b6d6151eb1bdbeecec5561fabd72&profile_id=175";
                break;
            case "Inverted":
                links[0] = "https://player.vimeo.com/external/502135645.hd.mp4?s=48948597eb2ef2e25bdec00755a8a97f4cff72c2&profile_id=175";
                links[1] = "https://player.vimeo.com/external/502136581.hd.mp4?s=e699a56e8ce71709a0743c1a1960d9b7bb829f2c&profile_id=175";
                links[2] = "https://player.vimeo.com/external/502140050.hd.mp4?s=2f92a55c12e34c4ac4965fdd619bf6614b33b7bc&profile_id=175";
                links[3] = "https://player.vimeo.com/external/502142549.hd.mp4?s=912eb7ea3d3e4400a68aa94c7ab01eb156e652af&profile_id=175";
                links[4] = "https://player.vimeo.com/external/502144943.hd.mp4?s=0b4306fce5f260912203d5837a0154c0b8fbdea0&profile_id=175";
                break;
            case "Normal":
                links[0] = "https://player.vimeo.com/external/502145826.hd.mp4?s=095dde910afd26eb61066ac304a19de9d3cb9211&profile_id=175";
                links[1] = "https://player.vimeo.com/external/502146711.hd.mp4?s=f3dea7e5dd775de6f707e65aab5e2674b07e0abf&profile_id=175";
                break;
        }
        return links;
    }
    private string[] SetOfInstructions()
    {
        string[] instructions = new string[7];

        switch (currentTutorial)
        {
            case "Base":
                instructions[0] = "1. Take 3 pieces and connect them to make one stack.";
                instructions[1] = "2. Take two of your stacks and connect them as shown.";
                instructions[2] = "3. Continue all the way around.";
                instructions[3] = "4. Squeeze the finished product into a cup shape.";
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
                instructions[1] = "2. To make your model more stable, glue down increased rows.";
                instructions[2] = "3. Continue like this all the way around. \n At the end, the amount of pieces has doubled.";
                instructions[3] = "4. Putting on a normal row above also increases stability.";
                instructions[4] = "5. This technique is used to create curved objects.";
                break;
            case "Inverted":
                instructions[0] = "1. Turn every piece to face inwards \n and put one piece over two tips.";
                instructions[1] = "2. Continue all the way around";
                instructions[2] = "3. Notice, that when applying a new row above it, \n the inverted row is covered completely.";
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
            //vimeo
            ChangeVimeoVideo(clipIndexInCurrentTutorial - 1);
            // youtube
            //ChangeYouTubeVideo(clipIndexInCurrentTutorial - 1);
            // desktop
            //ChangeVideo(clipIndexInCurrentTutorial.ToString());
            progressBar.value = clipIndexInCurrentTutorial;
            instructionsText.text = instructions[clipIndexInCurrentTutorial - 1];
        }
    }

    public void PreviousClip()
    {
        if (clipIndexInCurrentTutorial > 1)
        {
            clipIndexInCurrentTutorial -= 1;
            //vimeo
            ChangeVimeoVideo(clipIndexInCurrentTutorial - 1);
            // youtube
            //ChangeYouTubeVideo(clipIndexInCurrentTutorial - 1); // -1 because we count from 0 
            // desktop
            //ChangeVideo(clipIndexInCurrentTutorial.ToString());
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
