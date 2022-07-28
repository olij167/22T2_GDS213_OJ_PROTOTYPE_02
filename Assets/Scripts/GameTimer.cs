using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class GameTimer : MonoBehaviourPunCallbacks
{
    public GameObject gameOverPanel, continueButton;

    public TextMeshProUGUI gameOverText, loserText, loserSubText, continueText;
    TextMeshProUGUI timerText;

    public SetGameLength gameLength;
    public float gameTimer;
    float timerReset;

    public bool gameOver, isBuildUps;

    public Player tagger;

    public List<string> loserSubTextOptions;
    int randomSubText;

    public float minX, maxX, minY, maxY, minZ, maxZ;

    //public AudioClip gongClip;

    void Start()
    {
        randomSubText = Random.Range(0, loserSubTextOptions.Count);

        if (PhotonNetwork.IsMasterClient)
        {
            gameTimer = timerReset = gameLength.gameTimer;

            isBuildUps = gameLength.isBuildUps;

            photonView.RPC("SendGameTimer", RpcTarget.AllBuffered, gameTimer);
        }

        
        timerText = GetComponent<TextMeshProUGUI>();

        gameOver = false;

        //foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        //{
        //    if (!player.GetComponent<TagStatus>().enabled)
        //    {
        //        player.GetComponent<TagStatus>().enabled = true;
        //    }
        //}

    }

    void Update()
    {
        //tagger = GetTagger();

        if (!gameOver)
        {
            gameTimer -= Time.deltaTime;

            gameOverPanel.SetActive(false);
            continueButton.SetActive(false);
            continueText.enabled = false;
        }
        else if (!isBuildUps)
        {

            //foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            //{
            //    CapsuleCollider collider;

            //    if (player.GetComponent<CapsuleCollider>() && player.GetComponent<CapsuleCollider>().isTrigger)
            //    {
            //        collider = player.GetComponent<CapsuleCollider>();

            //        collider.enabled = false;
            //    }
            //}

            gameOverPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;

            gameOverText.text = "GAME OVER! \n";
            if (tagger != null)
            {
                loserText.text = tagger.NickName + " was IT";
            }
            else
            {
                loserText.text = "";
            }
            loserSubText.text = loserSubTextOptions[randomSubText];

            if (PhotonNetwork.IsMasterClient)
            {
                continueButton.SetActive(true);
                continueText.enabled = false;
            }
            else
            {
                continueText.enabled = true;
            }

            //foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            //{
            //    if (player.GetComponent<TagStatus>().enabled)
            //    {
            //        player.GetComponent<TagStatus>().enabled = false;
            //    }
            //}
        }
        else
        {

            gameOverPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;

            gameOverText.text = "GAME OVER! \n";


            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (!(bool)PhotonNetwork.PlayerList[i].CustomProperties["tagStatus"])
                {
                    loserText.text = "Free Team Win!";
                }
                else
                {
                    loserText.text = "It Wins!";
                    break;
                }
            }

            loserSubText.text = loserSubTextOptions[randomSubText];

            if (PhotonNetwork.IsMasterClient)
            {
                continueButton.SetActive(true);
                continueText.enabled = false;
            }
            else
            {
                continueText.enabled = true;
            }
        }

        timerText.text = gameTimer.ToString("00");


        if (gameTimer <= 0f || !GetFree())
        {
            gameOver = true;
            tagger = GetTagger();
            //GetComponent<AudioSource>().PlayOneShot(gongClip);
            //Debug.Log("Game Over");

           
        }
        else
        {
            gameOver = false;

            
        }
    }

    public void ContinueButton()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            //Destroy(player);
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
            player.GetComponent<CharacterController>().enabled = true;

        }

        //PhotonNetwork.LoadLevel("TagGame");
        gameTimer = timerReset;

        photonView.RPC("SendGameTimer", RpcTarget.AllBuffered, gameTimer, isBuildUps);

        //// Set new random tagger
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    photonView.RPC("SetAllFreeRPC", RpcTarget.AllBuffered);

        //    Player randomPlayer = PhotonNetwork.PlayerList[Random.Range(1, PhotonNetwork.PlayerList.Length)];

        //    photonView.RPC("SetTaggerRPC", RpcTarget.AllBuffered, randomPlayer);
        //}

        randomSubText = Random.Range(0, loserSubTextOptions.Count);

        gameOverPanel.SetActive(false);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("LobbyScene");

    }

    [PunRPC]
    public void SendGameTimer(float timer, bool gameMode)
    {

        gameTimer = timer;
        isBuildUps = gameMode;
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
                    //Debug.Log("Tagger is = " + PhotonNetwork.PlayerList[i].NickName);
                    return PhotonNetwork.PlayerList[i];
                }
            }
        }

        return null;
    }

    public bool GetFree()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].CustomProperties["tagStatus"] != null)
            {
                if (!(bool)PhotonNetwork.PlayerList[i].CustomProperties["tagStatus"])
                {
                    //Debug.Log("Tagger is = " + PhotonNetwork.PlayerList[i].NickName);
                    return true;
                }
            }
        }

        return false;
    }

    //public void OnEnable()
    //{
    //    PhotonNetwork.NetworkingClient.EventReceived += SendGameTime;
    //}
}
