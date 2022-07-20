using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PauseMenu : MonoBehaviourPunCallbacks
{
    public GameObject pauseMenu, playerListUI, playerListItem;

    public List<GameObject> playerListItemList;

    UnityEvent togglePauseMenu;

    public bool pauseMenuEnabled;

    public Color[] playerColoursArray;


    void Start()
    {
        //playerName = playerListItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //playerTagStatus = playerListItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        if (togglePauseMenu == null)
        {
            togglePauseMenu = new UnityEvent();
        }

        togglePauseMenu.AddListener(TogglePauseMenu);

        togglePauseMenu.Invoke();

        LogPlayerList();

        if (photonView.Owner.CustomProperties["tagStatus"] != null)
        {
            UpdatePlayerListTagStatus();
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            togglePauseMenu.Invoke();
        }

    }

    public void TogglePauseMenu()
    {
        switch (pauseMenuEnabled)
        {
            case true:
                {
                    pauseMenu.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
                }

            case false:
                {
                    pauseMenu.SetActive(true);
                    Cursor.lockState = CursorLockMode.Confined;
                    break;
                }
        }
    }

    public void LogPlayerList()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject newPlayerListItem = Instantiate(playerListItem, playerListUI.transform);

            if (!playerListItemList.Contains(newPlayerListItem))
            {
                playerListItemList.Add(newPlayerListItem);
            }

            PlayerListItem newItemScript = newPlayerListItem.GetComponent<PlayerListItem>();

            newItemScript.nameText.text = player.NickName;
            newItemScript.background.color = playerColoursArray[(int)player.CustomProperties["playerColour"]];

        }
    }

    public void UpdatePlayerListTagStatus()
    {
        foreach (GameObject playerListItem in playerListItemList)
        {
            switch ((bool)playerListItem.GetComponent<PlayerListItem>().player.CustomProperties["tagStatus"])
            {
                case false:
                    {
                        playerListItem.GetComponent<PlayerListItem>().tagText.text = "Free";
                        playerListItem.GetComponent<PlayerListItem>().tagText.color = Color.green;
                        break;
                    }

                case true:
                    {
                        playerListItem.GetComponent<PlayerListItem>().tagText.text = "IT";
                        playerListItem.GetComponent<PlayerListItem>().tagText.color = Color.red;
                        break;
                    }
            }

        }
    }


}


    //[PunRPC]
    //public void UpdatePlayerListTagStatusRPC(Player player, GameObject playerListItem)
    //{
       
    //}

