using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput, gameTimerInput;

    public SetGameLength gameLength;
    public GameObject lobbyPanel, roomPanel;
    public TextMeshProUGUI roomName, waitForHostText, waitForPlayersText;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;

    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public GameObject playButton;

    //public int nextPlayersTeam = 1;

    void Start()
    {
        roomPanel.SetActive(false);

        PhotonNetwork.JoinLobby();

        createInput.characterLimit = 12;
        gameTimerInput.contentType = TMP_InputField.ContentType.DecimalNumber;
    }

    public void CreateRoom()
    {
        if (createInput.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(createInput.text, new RoomOptions {MaxPlayers = 12, BroadcastPropsChangeToAll = true});
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
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
