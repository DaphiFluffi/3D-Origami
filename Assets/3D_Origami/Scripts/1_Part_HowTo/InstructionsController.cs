using TMPro;
using UnityEngine;

public class InstructionsController : MonoBehaviour
{
    // = default to avoid the following warning warning CS0649: Field is never assigned to, and will always have its default value `null'
    // warning appears because my variables are private
    // https://answers.unity.com/questions/60461/warning-cs0649-field-is-never-assigned-to-and-will.html answer by Delacrowa
    
    // UI Text the instructions are going to be displayed in 
    [SerializeField] private TextMeshProUGUI instructionsText = default;
    // reference to TutorialController Object
    private TutorialController tutorialController;
    // holds the instructions
    [SerializeField] private string[] instructions = default;
    
    void Awake()
    {
        // access TutorialController Script
        tutorialController = FindObjectOfType<TutorialController>();
        // take first instruction text in the beginning 
        instructionsText.text = instructions[0];
    }
    
    // public function gets called from next or previous button to change the text according to the current step
    public void ChangeInstructionText()
    {
        instructionsText.text = instructions[tutorialController.GetIndex()];
    }
}
