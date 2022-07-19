using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameTimer : MonoBehaviourPunCallbacks
{
    Hashtable timerProperties = new Hashtable();

    public GameObject gameOverPanel, continueButton;

    public const byte sendGameTimerEventCode = 1;

    public TextMeshProUGUI gameOverText, loserText, continueText;
    TextMeshProUGUI timerText;

    public SetGameLength gameLength;
    public float gameTimer;
    float timerReset;

    public bool gameOver;

    public Player tagger;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameTimer = timerReset = gameLength.gameTimer;

            photonView.RPC("SendGameTimer", RpcTarget.AllBuffered, gameTimer);


            //timerProperties.Add("gameTimer", gameTimer);
            //PhotonNetwork.CurrentRoom.SetCustomProperties(timerProperties);
        }

        
        timerText = GetComponent<TextMeshProUGUI>();

        gameOver = false;

        //if (!PhotonNetwork.IsMasterClient)
        //{
        //    gameTimer = (float)PhotonNetwork.MasterClient.CustomProperties["gameTimer"];

        //    timerProperties.Add("gameTimer", gameTimer);

        //}

    }

    void Update()
    {
        tagger = GetTagger();

        if (!gameOver)
        {
            gameTimer -= Time.deltaTime;

            gameOverPanel.SetActive(false);
            continueButton.SetActive(false);
            continueText.enabled = false;
        }
        else
        {
            gameOverPanel.SetActive(true);

            gameOverText.text = "GAME OVER! \n";
            loserText.text = GetTagger().NickName + " was IT"; 

            if (PhotonNetwork.IsMasterClient)
            {
                continueButton.SetActive(true);
                continueText.enabled = false;
            }
            else
            {
                continueText.enabled = true;
            }
            // + PhotonNetwork.PlayerList[i].NickName + " was the tagger at the end... \n what a loser";
            //if (PhotonNetwork.IsMasterClient)
            //{
            //    for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            //    {
            //        if ((bool)PhotonNetwork.PlayerList[i].CustomProperties["tagProperties"])
            //        {
            //            gameOverPanel.SetActive(true);

            //        }
            //    }

            //}

            //if (photonView.IsMine)
            //{
            //    if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["tagProperties"])
            //    {
            //        gameOverText.text += "You were it... \n what a loser";
            //    }
            //    else
            //    {
            //        gameOverText.text += "You are free for another day \n Winner!";
            //    }
            //}
        }

        timerText.text = gameTimer.ToString("00");


        if (gameTimer <= 0f)
        {
            gameOver = true;
            Debug.Log("Game Over");
            //PhotonNetwork.CurrentRoom.SetCustomProperties(timerProperties);
        }
        else
        {
            gameOver = false;
        }
    }

    public void ContinueButton()
    {
        gameTimer = timerReset;

        photonView.RPC("SendGameTimer", RpcTarget.AllBuffered, gameTimer);

        gameOverPanel.SetActive(false);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("LobbyScene");

    }

    [PunRPC]
    public void SendGameTimer(float timer)
    {

        gameTimer = timer;
            //gameTimer = (float)PhotonNetwork.CurrentRoom.CustomProperties["gameTimer"];
            //PhotonNetwork.CurrentRoom.SetCustomProperties(timerProperties);
        
    }

    public Player GetTagger()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].CustomProperties["tagStatus"] != null)
            {
                if ((bool)PhotonNetwork.PlayerList[i].CustomProperties["tagStatus"])
                {
                    Debug.Log("Tagger = " + PhotonNetwork.PlayerList[i].NickName);
                    return PhotonNetwork.PlayerList[i];
                }
            }
        }

        return null;
    }

    //public void OnEnable()
    //{
    //    PhotonNetwork.NetworkingClient.EventReceived += SendGameTime;
    //}
}
