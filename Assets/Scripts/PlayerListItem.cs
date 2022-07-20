using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    public Player player;

    public TextMeshProUGUI nameText, tagText;

    public Image background;

    public Color[] playerColoursArray;

    public void SetPlayerInfo(Player _player)
    {
        nameText.text = _player.NickName;
        player = _player;

        background.color = playerColoursArray[(int)player.CustomProperties["playerColour"]];
        SetTagStatusUI(_player);

        // UpdatePlayerItem(player);
    }

    public void SetTagStatusUI(Player _player)
    {
        tagText.text = PlayerTagStatus(_player);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            SetTagStatusUI(targetPlayer);
        }
    }


    public string PlayerTagStatus(Player _player)
    {
        switch (_player.CustomProperties["tagStatus"])
        {
            case false:
                {
                    
                    tagText.color = Color.green;
                    //break;
                    return tagText.text = "Free";
                }

            case true:
                {
                    tagText.color = Color.red;
                    //break;
                    return tagText.text = "IT";
                }

            default:
                {
                    tagText.color = Color.magenta;
                    //break;
                    return tagText.text = "ERROR";
                }
        }
    }

    //public void LogPlayerList()
    //{
    //    foreach (Player player in PhotonNetwork.PlayerList)
    //    {
    //        GameObject newPlayerListItem = Instantiate(playerListItem, playerListUI.transform);

    //        if (!playerListItemList.Contains(newPlayerListItem))
    //        {
    //            playerListItemList.Add(newPlayerListItem);
    //        }

    //        PlayerListItem newItemScript = newPlayerListItem.GetComponent<PlayerListItem>();

    //        newItemScript.nameText.text = player.NickName;
    //        newItemScript.background.color = playerColoursArray[(int)player.CustomProperties["playerColour"]];

    //    }
    //}

    //public void UpdatePlayerListTagStatus(Player _player)
    //{

    //    switch (_player.CustomProperties["tagStatus"])
    //    {
    //        case false:
    //            {
    //                tagText.text = "Free";
    //                tagText.color = Color.green;
    //                break;
    //            }

    //        case true:
    //            {
    //                tagText.text = "IT";
    //                tagText.color = Color.red;
    //                break;
    //            }

    //        default:
    //            {
    //                tagText.text = "ERROR";
    //                tagText.color = Color.magenta;
    //                break;
    //            }
    //    }

        
    //}
}
