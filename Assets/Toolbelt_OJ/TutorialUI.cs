using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "TutotrialUI")]
public class TutorialUI : ScriptableObject
{
    //public Sprite tutorialIcon;

    [TextArea(3, 10)] // for inspector
    public string tutorialInstructions;
    //public bool anyButtonToProceed;

    //public AudioClip voiceOver;

    public List<KeyCode> tutorialInputList;
}
