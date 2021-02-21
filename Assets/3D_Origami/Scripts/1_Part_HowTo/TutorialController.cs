
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject[] folds = new GameObject[7];
    [SerializeField] private Button pauseButton = default;
    [SerializeField] private Button nextButton = default;
    [SerializeField] private Button nextSceneButton = default;
    [SerializeField] private Button previousButton = default;
    [SerializeField] private Sprite Unpause = default;
    [SerializeField] private Sprite Pause = default;
    [SerializeField] private Slider progressBar = default;
    [SerializeField] private TextMeshProUGUI progressText = default;
    private int index;
    private bool paused;

    void Awake()
    {
        index = 0; 
        //play the first animation
        folds[index].SetActive(true); 
        folds[index].GetComponent<Animator>().SetFloat("animSpeed", 1);
        // indicate that we are at step 1
        progressBar.value = 1; 
        progressText.text = progressBar.value + " / " + progressBar.maxValue;
        // if it is set as true in the beginning, the PauseAnimation() method flips it to false
        paused = true; 
        PauseOrUnpauseAnimation();
    }

    void Update() // don't show previous button at the first step, don't show next button at the last step 
    {
        if (index == 0)
        {
            previousButton.gameObject.SetActive(false);
        } 
        else if (index == folds.Length - 1)
        {
            nextButton.gameObject.SetActive(false);
            nextSceneButton.gameObject.SetActive(true);
        }
        else
        {
            previousButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
            nextSceneButton.gameObject.SetActive(false);
        }
    }
    
    //How to use sliders https://www.youtube.com/watch?v=HQ8Tttcksu4
    public void AnimationSpeed(float sliderSpeed) //value of the slider will be assigned to the sliderSpeed Variable
    {
        sliderSpeed /= 2f;
        //https://forum.unity.com/threads/losing-animator-state.307667/
        folds[index].GetComponent<Animator>().SetFloat("animSpeed", sliderSpeed); 
    }
    
    void ProgressBar()
    {
        progressBar.value = index + 1;
        progressText.text = progressBar.value + " / " + progressBar.maxValue;
    }
    
    public void NextAnimation()
    {
        if (index != folds.Length - 1)
        {
            folds[index].SetActive(false);
            folds[index + 1].SetActive(true);
            // slider speed has to persist on all different animators
            // you can only set Animation Speed at runtime trough modifiers ("animSpeed" in this case)
            //https://answers.unity.com/questions/1472587/change-the-speed-of-a-specific-animation-of-an-ani.html
            // give the next animation the speed of the current animation when changing to it 
            var nextAnim = folds[index + 1].GetComponent<Animator>();
            nextAnim.SetFloat("animSpeed", folds[index].GetComponent<Animator>().GetFloat("animSpeed")); 
            // play the first and only animation each object has 
            nextAnim.Play(0, 0, 0); 
            index += 1;
            ProgressBar();
            //if you paused at the current step, the next step will be played without pause 
            paused = true; 
            PauseOrUnpauseAnimation();
        }
    }
    
    public void PreviousAnimation()
    {
        if (index != 0)
        {
            folds[index].SetActive(false);
            folds[index - 1].SetActive(true);
            // give the previous animation the speed of the current animation when changing to it 
            var prevAnim = folds[index - 1].GetComponent<Animator>();
            prevAnim.SetFloat("animSpeed", folds[index].GetComponent<Animator>().GetFloat("animSpeed"));
            // play the first and only animation each object has 
            prevAnim.Play(0, 0, 0); 
            index -= 1;
            ProgressBar();
            paused = true;
            PauseOrUnpauseAnimation();
        }
    }

    public void ReplayAnimation()
    {
        // you should only be able to replay the animation, when the animation is not paused
        if (paused)
        {
            UnpauseAnimation();
        }

        // https://docs.unity3d.com/ScriptReference/Animator.Play.html
        var anim = folds[index].GetComponent<Animator>();
        // play the first and only animation each object has 
        anim.Play(0, 0, 0);
    }

    public void PauseOrUnpauseAnimation()
    {
        if (paused) 
        {
            UnpauseAnimation();
        }
        else 
        {
            PauseAnimation();
        }
    }
    
    private void PauseAnimation()
    {
        //pause animation
        folds[index].GetComponent<Animator>().speed = 0f; 
        // set sprite to Unpause Icon 
        pauseButton.image.overrideSprite = Unpause;
        paused = true;
    }
    
    private void UnpauseAnimation()
    {
        //if there is an animator
        if (folds[index].GetComponent<Animator>() != null)
        {
            // unpause animation
            folds[index].GetComponent<Animator>().speed = 1f; //unpauseAnimation
        }
        // set sprite to Pause Icon 
        pauseButton.image.overrideSprite = Pause; 
        paused = false;
    }

    public int GetIndex()
    {
        return index; 
    }

}
