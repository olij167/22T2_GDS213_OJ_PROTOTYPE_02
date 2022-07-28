using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerStartPrompt : MonoBehaviourPunCallbacks
{
    TextMeshProUGUI runText;

    public TextMeshProUGUI tagImpactText;

    public List<string> buildUpsImpactTextOptions;

    int buildUpsTextChoice;

    public Image tagImpactBackground, tagImpactImage;

    public float textDisplayTimer, textFadeInTime = 2f, textFadeOutTime = 2f;
    public float imageDisplayTimer, imageFadeInTime = 1f, imageFadeOutTime = 2f;
    float textTimerReset, imageTimerReset;

    public bool displayText;

    public bool displayImage;

    public GameTimer gameTimer;

    Color originalBackgroundColour, originalImageColour,invisBackgroundColour, invisImageColour;


    void Start()
    {
        runText = GameObject.FindGameObjectWithTag("RunText").GetComponent<TextMeshProUGUI>();

        originalBackgroundColour = tagImpactBackground.color;
        originalImageColour = tagImpactImage.color;

        invisBackgroundColour = new Color(originalBackgroundColour.r, originalBackgroundColour.g, originalBackgroundColour.b, 0f);
        invisImageColour = new Color(originalImageColour.r, originalImageColour.g, originalImageColour.b, 0f);

        tagImpactBackground.color = invisBackgroundColour;
        tagImpactImage.color = invisImageColour;

        textTimerReset = textDisplayTimer;
        imageTimerReset = imageDisplayTimer;

        if (gameTimer.isBuildUps)
        {
            buildUpsTextChoice = Random.Range(0, buildUpsImpactTextOptions.Count);
        }
    }

    void Update()
    {
        if (displayText)
        {
            
            textDisplayTimer -= Time.deltaTime;

            if (textDisplayTimer >= textTimerReset - textFadeInTime)
            {
                runText.alpha = Mathf.Lerp(0f, 1f, textTimerReset - textDisplayTimer);

            }

            if (textDisplayTimer <= textFadeOutTime)
            {
                runText.alpha = Mathf.Lerp(0f, 1f, textDisplayTimer);
            }

            if (textDisplayTimer <= 0f)
            {
                displayText = false;
            }
        }
        else
        {
            runText.enabled = false;

            textDisplayTimer = textTimerReset;
        }

        if (displayImage)
        {
            tagImpactBackground.gameObject.SetActive(true);

            if (gameTimer.GetTagger() != null && !gameTimer.isBuildUps)
            {
                tagImpactText.text = gameTimer.GetTagger().NickName + " is IT";
            }
            else
            {
                tagImpactText.text = buildUpsImpactTextOptions[buildUpsTextChoice];
            }

            imageDisplayTimer -= Time.deltaTime;

            if (imageDisplayTimer >= imageTimerReset - imageFadeInTime)
            {

                tagImpactBackground.color = Color.Lerp(invisBackgroundColour, originalBackgroundColour, imageTimerReset - imageDisplayTimer);
                tagImpactImage.color = Color.Lerp(invisImageColour, originalImageColour, imageTimerReset - imageDisplayTimer);

                tagImpactText.alpha = Mathf.Lerp(0f, 1f, textTimerReset - textDisplayTimer);

            }

            if (imageDisplayTimer <= imageFadeOutTime)
            {
                tagImpactBackground.color = Color.Lerp(invisBackgroundColour, originalBackgroundColour, imageDisplayTimer);
                tagImpactImage.color = Color.Lerp(invisImageColour, originalImageColour, imageDisplayTimer);

                tagImpactText.alpha = Mathf.Lerp(0f, 1f, textDisplayTimer);
            }

            if (imageDisplayTimer <= 0f)
            {
                displayImage = false;
            }
        }
        else
        {
            tagImpactBackground.gameObject.SetActive(false);

            imageDisplayTimer = imageTimerReset;

            if (gameTimer.isBuildUps)
            {
                buildUpsTextChoice = Random.Range(0, buildUpsImpactTextOptions.Count);
            }
        }

    }

    public void SetText()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["tagStatus"] != null)
        {

            switch (PhotonNetwork.LocalPlayer.CustomProperties["tagStatus"])
            {
                case true:
                    {
                        runText.text = "You are IT. \n Tag someone!";
                        break;
                    }
                case false:
                    {
                        runText.text = "You are Free. \n Avoid IT!";
                        break;
                    }
                default:
                    {
                        runText.text = "Error";
                        break;
                    }
            }

            runText.enabled = true;

            displayText = true;
        }        
    }
}
