using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerTimer : MonoBehaviour
{
    Hashtable playerTimer = new Hashtable();

    TextMeshProUGUI timerText;

    public float tagTimer;
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();

        playerTimer["tagTimer"] = tagTimer;
        PhotonNetwork.CurrentRoom.SetCustomProperties(playerTimer);
        

    }

    void Update()
    {
        timerText.text = tagTimer.ToString("00");

        tagTimer += Time.deltaTime;
    }
}
