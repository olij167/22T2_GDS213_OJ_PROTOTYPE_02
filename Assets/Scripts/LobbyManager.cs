using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput, gameTimerInput;

    public SetGameLength gameLength;
    public GameObject lobbyPanel, roomPanel;
    public TextMeshProUGUI roomName, waitForHostText, waitForPlayersText, gamemodeText;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;
    Hashtable roomProperties = new Hashtable() { ["GameMode"] = false };

    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public GameObject playButton, gameModeParent;
    public Button gamemodeButton;

    //bool gameMode;
    //string gameModeText;

    //public int nextPlayersTeam = 1;

    void Start()
    {
        roomPanel.SetActive(false);

        PhotonNetwork.JoinLobby();

        createInput.characterLimit = 12;
        gameTimerInput.contentType = TMP_InputField.ContentType.DecimalNumber;
        gameLength.isBuildUps = false;

        
    }

    public void CreateRoom()
    {
        if (createInput.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(createInput.text, new RoomOptions {MaxPlayers = 12, BroadcastPropsChangeToAll = true, CustomRoomProperties = roomProperties});
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();

        //if (PhotonNetwork.IsMasterClient)
        //{

        //}
        roomProperties["GameMode"] = gameLength.isBuildUps;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        //roomProperties["GameMode"] = gameLength.isBuildUps;

        UpdateGameMode();
        
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }

            playerItemsList.Add(newPlayerItem);
        }

        //UpdateTeam();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            waitForHostText.enabled = false;
            waitForPlayersText.enabled = true;
            playButton.SetActive(false);
            gameModeParent.SetActive(true);

            gameTimerInput.gameObject.SetActive(true);

            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                waitForPlayersText.enabled = false;
                playButton.SetActive(true);
                gameTimerInput.gameObject.SetActive(true);
            }
        }
        else
        {
            waitForPlayersText.enabled = false;
            playButton.SetActive(false);
            gameTimerInput.gameObject.SetActive(false);
            gameModeParent.SetActive(false);
            
            waitForHostText.enabled = true;
        }

    }

    public void OnClickPlayButton()
    {
        if (gameTimerInput.text.Length >= 2)
        {
            gameLength.gameTimer = System.Convert.ToInt32(gameTimerInput.text);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("TagGame");
        }
    }

    public void OnClickToggleBuildUps()
    {

        if (gameLength.isBuildUps)
        {
            //gamemodeText.text = "Classic"; 
            gameLength.isBuildUps = false;
        }
        else
        {
            //gamemodeText.text = "Build-Ups";
            gameLength.isBuildUps = true;
        }

        roomProperties["GameMode"]  = gameLength.isBuildUps;
        //gameMode = gameLength.isBuildUps;

        //gameModeText = gamemodeText.text;

        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }

    public void UpdateGameMode()
    {
        
        //gameLength.isBuildUps = gameMode;
        //roomProperties["GameMode"] = gameMode;

        string gameModeText;

        Debug.Log("Updating gamemode: " + gameLength.isBuildUps);

        if (roomProperties["GameMode"] != null)
        {
            if ((bool)roomProperties["GameMode"])
            {
                gameModeText = "Build-Ups";
            }
            else
            {
                gameModeText = "Classic";
            }

            gamemodeText.text = gameModeText;
        }

        //PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        //;
    }

    //public void UpdateTeam()
    //{
    //    if (nextPlayersTeam == 1)
    //    {
    //        nextPlayersTeam = 2;
    //    }
    //    else
    //    {
    //        nextPlayersTeam = 1;
    //    }
    //}
}
