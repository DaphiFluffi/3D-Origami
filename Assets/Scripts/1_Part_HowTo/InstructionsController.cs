using TMPro;
using UnityEngine;

public class InstructionsController : MonoBehaviour
{
    // = default to avoid the following warning warning CS0649: Field is never assigned to, and will always have its default value `null'
    //warning appears because my variables are private
    // https://answers.unity.com/questions/60461/warning-cs0649-field-is-never-assigned-to-and-will.html answer by Delacrowa
    [SerializeField]
    private TextMeshProUGUI instructionsText = default;
    [SerializeField]
    private GameObject tutorialControllerObject = default;
    private TutorialController tutorialControllerScript;
    private string [] instructions = 
    {"1. Fold the paper in half horizontally.", 
        "2. Fold it vertically and open it up again \n to get a vertical crease.", 
        "3. Fold both sides towards that crease.", 
        "4. Turn the paper on its back.",
        "5. Fold the small corners inwards.",
        "6. Fold up the two protruding flaps.",
        "7. Fold it in half and you are done!"
    };
    
    void Awake()
    {
        tutorialControllerScript = tutorialControllerObject.GetComponent<TutorialController>();
        instructionsText.text = instructions[0];
    }
    
    public void ChangeInstructionText()
    {
        instructionsText.text = instructions[tutorialControllerScript.GetIndex()];
    }
}
