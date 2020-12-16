using TMPro;
using UnityEngine;

public class InstructionsController : MonoBehaviour
{
    // = default to avoid the following warning warning CS0649: Field is never assigned to, and will always have its default value `null'
    //warning appears because my variables are private
    // https://answers.unity.com/questions/60461/warning-cs0649-field-is-never-assigned-to-and-will.html answer by Delacrowa
    [SerializeField] private TextMeshProUGUI instructionsText = default;
    private TutorialController tutorialController;
    [SerializeField] private string[] instructions = default;
    
    void Awake()
    {
        tutorialController = FindObjectOfType<TutorialController>();
        instructionsText.text = instructions[0];
    }
    
    public void ChangeInstructionText()
    {
        instructionsText.text = instructions[tutorialController.GetIndex()];
    }
}
