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

}
