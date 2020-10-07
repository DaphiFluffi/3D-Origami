
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] folds = new GameObject[7];
    [SerializeField]
    private Button pauseButton = default;
    [SerializeField]
    private Sprite Unpause = default;
    [SerializeField]
    private Sprite Pause = default;
    [SerializeField]
    private Slider progressBar = default;
    private int _index;
    private bool _paused;
    private Animator m_Animator;
    void Awake()
    {
        folds[0].SetActive(true);
        progressBar.value = 1; 
        _index = 0;
        _paused = true; // if it is set as true in the beginning, the PauseAnimation() method flips it to false
        PauseOrUnpauseAnimation();
    }
    
    void Start()
    {
        //Get the animator, attached to the GameObject you are intending to animate.
        m_Animator = folds[_index].GetComponent<Animator>();
    }

    //How to use sliders https://www.youtube.com/watch?v=HQ8Tttcksu4
    public void AnimationSpeed(float sliderSpeed) //value of the slider will be assigned to the sliderSpeed Variable
    {
        m_Animator.speed = sliderSpeed;
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
            _index += 1;
            ProgressBar();

        }
    }
    
    public void PreviousAnimation()
    {
        if (_index != 0)
        {
            folds[_index].SetActive(false);
            folds[_index - 1].SetActive(true);
            _index -= 1;
            ProgressBar();

        }
    }

    public void ReplayAnimation()
    {
        // https://docs.unity3d.com/ScriptReference/Animator.Play.html
        var anim = folds[_index].GetComponent<Animator>();
        anim.Play(0,0,0); // play the first and only animation each object has 
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
        folds[_index].GetComponent<Animator>().speed = 1f; //unpauseAnimation
        pauseButton.image.overrideSprite = Pause; //"Pause";
        _paused = false;
    }

    public int GetIndex()
    {
        return _index; 
    }

}
