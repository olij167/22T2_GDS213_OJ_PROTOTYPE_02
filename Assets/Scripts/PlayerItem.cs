using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playerName;

    Image backgroundImage;
    //public Color highlightColour;
    public GameObject leftArrowButton, rightArrowButton;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable() { ["playerColour"] = 0 };
    public Color playerColour;
    public Color[] playerColoursArray;

    //public int playerTeam;

    Player player;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        //playerProperties["playerColour"] = 0;
        //playerProperties["tagStatus"] = "Free";
        backgroundImage.color = playerColour;

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;

        UpdatePlayerItem(player);
    }

    public void ApplyLocalChanges()
    {
        backgroundImage.color = playerColour;
        leftArrowButton.SetActive(true);
        rightArrowButton.SetActive(true);
    }

    public void OnClickLeftArrow()
    {
        if ((int)playerProperties["playerColour"] == 0)
        {
            playerProperties["playerColour"] = playerColoursArray.Length - 1;
        }
        else
        {
            playerProperties["playerColour"] = (int)playerProperties["playerColour"] - 1;
        }

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);

    }

    public void OnClickRightArrow()
    {
        if ((int)playerProperties["playerColour"] == playerColoursArray.Length - 1)
        {
            playerProperties["playerColour"] = 0;
        }
        else
        {
            playerProperties["playerColour"] = (int)playerProperties["playerColour"] + 1;
        }

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("playerColour"))
        {
            backgroundImage.color = playerColour = playerColoursArray[(int)player.CustomProperties["playerColour"]];
            playerProperties["playerColour"] = (int)player.CustomProperties["playerColour"];
        }
        else
        {
            playerProperties["playerColour"] = 0;
        }
    }

    //[PunRPC]
    //void RPC_GetTeam()
    //{
    //    playerTeam = FindObjectOfType<LobbyManager>().nextPlayersTeam;
    //    FindObjectOfType<LobbyManager>().UpdateTeam();
    //    photonView.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, playerTeam);

    //    Debug.Log(photonView.name + " is on team " + playerTeam);
    //}

    //[PunRPC]
    //void RPC_SentTeam(int whichTeam)
    //{
    //    playerTeam = whichTeam;
    //}

}
