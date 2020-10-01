using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] folds = new GameObject[7];
    [SerializeField]
    private TextMeshProUGUI pauseButton;
    private int _index;
    private bool _paused;
    void Awake()
    {
        folds[0].SetActive(true);
        _index = 0;
        _paused = true; // if it is set as true in the beginning, the PauseAnimation() method flips it to false
        PauseOrUnpauseAnimation();
    }
    
    public void NextAnimation()
    {
        if (_index != folds.Length - 1)
        {
            folds[_index].SetActive(false);
            folds[_index + 1].SetActive(true);
            _index += 1;
        }
    }
    
    public void PreviousAnimation()
    {
        if (_index != 0)
        {
            folds[_index].SetActive(false);
            folds[_index - 1].SetActive(true);
            _index -= 1;
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
        pauseButton.text = "Unpause";
        _paused = true;
    }
    
    private void UnpauseAnimation()
    {
        folds[_index].GetComponent<Animator>().speed = 1f; //unpauseAnimation
        pauseButton.text = "Pause";
        _paused = false;
    }
    
  /*  Animator m_Animator;
    //Value from the slider, and it converts to speed level
    float m_MySliderValue;

    void Start()
    {
        //Get the animator, attached to the GameObject you are intending to animate.
        m_Animator = folds[_index].GetComponent<Animator>();
    }

    void OnGUI()
    {
        //Create a Label in Game view for the Slider
        GUI.Label(new Rect(0, 25, 40, 60), "Speed");
        //Create a horizontal Slider to control the speed of the Animator. Drag the slider to 1 for normal speed.

        m_MySliderValue = GUI.HorizontalSlider(new Rect(45, 25, 200, 800), m_MySliderValue, 0.0F, 1.0F);
        //Make the speed of the Animator match the Slider value
        m_Animator.speed = m_MySliderValue;
    }*/

}
