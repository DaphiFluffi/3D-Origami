
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
    private int _index;
    private bool _paused;
    private Animator m_Animator;
    
    void Awake()
    {
        _index = 0; 
        folds[_index].SetActive(true); //play the first animation
        folds[_index].GetComponent<Animator>().SetFloat("animSpeed", 1);
        progressBar.value = 1; // indicate that we are at step 1
        _paused = true; // if it is set as true in the beginning, the PauseAnimation() method flips it to false
        PauseOrUnpauseAnimation();
    }

    void Update() // don't show previous button at the first step, don't show next button at the last step 
    {
        if (_index == 0)
        {
            previousButton.gameObject.SetActive(false);
        } 
        else if (_index == folds.Length - 1)
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
        //https://forum.unity.com/threads/losing-animator-state.307667/
        //folds[_index].GetComponent<Animator>().speed = sliderSpeed;
        folds[_index].GetComponent<Animator>().SetFloat("animSpeed", sliderSpeed); 
    }
    
    void ProgressBar()
    {
        progressBar.value = _index + 1; 
    }
    
    public void NextAnimation()
    {
        if (_index != folds.Length - 1)
        {
            folds[_index].SetActive(false);
            folds[_index + 1].SetActive(true);
            // slider speed has to persist on all different animators
            // you can only set Animation Speed at runtime trough modifiers ("animSpeed" in this case)
            //https://answers.unity.com/questions/1472587/change-the-speed-of-a-specific-animation-of-an-ani.html
            var nextAnim = folds[_index + 1].GetComponent<Animator>();
            nextAnim.SetFloat("animSpeed", folds[_index].GetComponent<Animator>().GetFloat("animSpeed")); 
            nextAnim.Play(0, 0, 0); // play the first and only animation each object has 
            Debug.Log("next " + folds[_index].GetComponent<Animator>().GetFloat("animSpeed"));
            _index += 1;
            ProgressBar();
            _paused = true; //if you paused at the current step, the next step will be played without pause 
            PauseOrUnpauseAnimation();
        }
    }
    
    public void PreviousAnimation()
    {
        if (_index != 0)
        {
            folds[_index].SetActive(false);
            folds[_index - 1].SetActive(true);
            var prevAnim = folds[_index - 1].GetComponent<Animator>();
            prevAnim.SetFloat("animSpeed", folds[_index].GetComponent<Animator>().GetFloat("animSpeed"));
            prevAnim.Play(0, 0, 0); // play the first and only animation each object has 
            Debug.Log("previous " + folds[_index].GetComponent<Animator>().GetFloat("animSpeed"));
            _index -= 1;
            ProgressBar();
            _paused = true;
            PauseOrUnpauseAnimation();
        }
    }

    public void ReplayAnimation()
    {
        if (_paused == false) // you should only be able to replay the animation, when the animation is not paused
        {
            // https://docs.unity3d.com/ScriptReference/Animator.Play.html
            var anim = folds[_index].GetComponent<Animator>();
            anim.Play(0, 0, 0); // play the first and only animation each object has 
        }
    }

    public void PauseOrUnpauseAnimation()
    {
        if (_paused) 
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
        folds[_index].GetComponent<Animator>().speed = 0f; //pauseAnimation
        pauseButton.image.overrideSprite = Unpause;//"Unpause";
        _paused = true;
    }
    
    private void UnpauseAnimation()
    {
        //if there is an animator
        if (folds[_index].GetComponent<Animator>() != null)
        {
            folds[_index].GetComponent<Animator>().speed = 1f; //unpauseAnimation
        }
        pauseButton.image.overrideSprite = Pause; //"Pause";
        _paused = false;
    }

    public int GetIndex()
    {
        return _index; 
    }

}
