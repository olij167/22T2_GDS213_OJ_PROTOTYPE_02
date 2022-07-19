using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField usernameInput;
    public TextMeshProUGUI buttonText;

    //public PlayerItem playerItem;
    //public Transform playerItemParent;

    public void OnClickConnect()
    {
        usernameInput.characterLimit = 10;
        if (usernameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = usernameInput.text;
            buttonText.text = "Connecting...";

            //playerItem.SetPlayerInfo(PhotonNetwork.LocalPlayer);
            //playerItem.ApplyLocalChanges();

            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("LobbyScene");
        //PhotonNetwork.JoinRandomOrCreateRoom();
        //PhotonNetwork.JoinRandomRoom();
    }

    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    CreateRoom();
    //}

    //private void CreateRoom()
    //{
    //    RoomOptions roomOptions = new RoomOptions();
    //    PhotonNetwork.CreateRoom(null, roomOptions, null);
    //}

    //public override void OnJoinedRoom()
    //{
    //    PhotonNetwork.LoadLevel("TagGame");
    //}
}
