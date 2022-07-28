using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TutorialUIController : MonoBehaviour
{
    public TextMeshProUGUI tutorialInstructionsText;
    
    public List<TutorialUI> tutorialUIList;
    public int count = 0;

    public bool tutorialComplete, inputPerformed;

    public GameObject leaveButton;

    private void Start()
    {
        leaveButton.SetActive(false);
        tutorialInstructionsText.text = tutorialUIList[count].tutorialInstructions;
    }

    //void ProgressTutorial()
    //{

    //    if (count + 1 >= tutorialUIList.Count)
    //    {
    //        tutorialComplete = true;
    //    }

    //    tutorialInstructionsText.text = tutorialUIList[count].tutorialInstructions;


    //    tutorialUIList[count].tutorialStepComplete = true;
    //    count++;

    //}




    void Update()
    {
        if (!tutorialComplete)
        {
            //if (tutorialUIList[count].anyButtonToProceed)
            //{
            //    if (Input.anyKeyDown)
            //    {
            //        inputPerformed = true;
            //    }
            //}

            if (tutorialUIList[count].tutorialInputList.Count > 0)
            {
                foreach (KeyCode tutorialInput in tutorialUIList[count].tutorialInputList)
                {
                    if (Input.GetKeyDown(tutorialInput))
                    {
                        inputPerformed = true;

                    }
                }
            }


            if (count + 1 < tutorialUIList.Count)
            {
                if (inputPerformed)
                {

                    count += 1;
                    tutorialInstructionsText.text = tutorialUIList[count].tutorialInstructions;
                    inputPerformed = false;

                }
            }
            else tutorialComplete = true;
        }
        
        if (tutorialComplete)
        {
            leaveButton.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }



    }
}
