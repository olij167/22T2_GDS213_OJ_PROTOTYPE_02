using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PauseMenu : MonoBehaviourPunCallbacks
{
    public GameObject pauseMenu, playerListItem;
    public Transform playerListUI;

    public List<PlayerListItem> playerListItemList;

    UnityEvent togglePauseMenu;

    public bool pauseMenuEnabled;

   // private GridLayoutGroup gridLayout;

    public GameTimer gameTimer;

    void Start()
    {
        //playerName = playerListItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //playerTagStatus = playerListItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        //gridLayout = playerListUI.GetComponent<GridLayoutGroup>();

        if (togglePauseMenu == null)
        {
            togglePauseMenu = new UnityEvent();
        }

        togglePauseMenu.AddListener(TogglePauseMenu);

        togglePauseMenu.Invoke();

        //LogPlayerList();

        //if (photonView.Owner.CustomProperties["tagStatus"] != null)
        //{
        //    UpdatePlayerListTagStatus();
        //}
    }

    void Update()
    {
        if (!gameTimer.gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                togglePauseMenu.Invoke();
            }
        }
        else
        {
            pauseMenu.SetActive(false);
        }

    }

    public void TogglePauseMenu()
    {

        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;

        }

        else
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;

        }

    }

    public void UpdatePlayerListUI()
    {
        foreach (PlayerListItem item in playerListItemList)
        {
            Destroy(item.gameObject);
        }

        playerListItemList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerListItem newPlayerListItem = Instantiate(playerListItem.gameObject, playerListUI).GetComponent<PlayerListItem>();
            newPlayerListItem.SetPlayerInfo(player.Value);

            //if (player.Value == PhotonNetwork.LocalPlayer)
            //{
            //    newPlayerListItem.SetTagStatusUI(player.Value);
            //}

            playerListItemList.Add(newPlayerListItem);
        }

        //if (playerListUI.childCount > 9)
        //{
        //    gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        //    gridLayout.constraintCount = 3;
        //}
        //else
        //{
        //    gridLayout.constraint = GridLayoutGroup.Constraint.Flexible;
        //}
    }

    public override void OnPlayerPropertiesUpdate(Player newPlayer, Hashtable changedProps)
    {
        UpdatePlayerListUI();
    }

    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    UpdatePlayerListUI();
    //}
}

