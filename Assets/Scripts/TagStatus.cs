using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TagStatus : MonoBehaviourPunCallbacks
{

    public TextMeshProUGUI tagStatusText;

    Hashtable tagProperties = new Hashtable();

    bool tagStatus = false;

    //GameObject tagObject;
    //OwnershipTransfer ownership;

    private void Start()
    {
        //tagObject = GameObject.FindGameObjectWithTag("TagObject");
        //ownership = tagObject.GetComponent<OwnershipTransfer>();

        tagProperties["tagStatus"] = tagStatus;


        if (PhotonNetwork.IsMasterClient)
        {
            //ownership.ChangeOwner();
            //Player randomPlayer = PhotonNetwork.PlayerList[Random.Range(1, PhotonNetwork.PlayerList.Length)];

            SetRandomTaggedPlayer();
        }

        PhotonNetwork.SetPlayerCustomProperties(tagProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (photonView.Owner == targetPlayer)
        {
            UpdateTagStatus(targetPlayer);
        }
    }


    public void UpdateTagStatus(Player player)
    {
        //if (tagObject.GetPhotonView().AmOwner)
        //{
        //    tagStatus = true;
        //}
        //else
        //{
        //    tagStatus = false;
        //}
        switch (player.CustomProperties["tagStatus"])
        //switch (tagStatus)
        {
            case false:
                {
                    tagStatusText.text = "Free";
                    tagStatusText.color = Color.green;
                    break;
                }

            case true:
                {
                    tagStatusText.text = "Tagger";
                    tagStatusText.color = Color.red;
                    break;
                }

            default:
                {
                    tagStatusText.text = "ERROR";
                    tagStatusText.color = Color.magenta;
                    break;
                }
        }

        //Debug.Log("status for '" + player.NickName + "' has been updated \n Is the tagger? ... " + player.CustomProperties["tagStatus"]);

    }


    void SetRandomTaggedPlayer()
    {

        tagStatus = true;

        tagProperties["tagStatus"] = tagStatus;
        //ownership.ChangeOwner();

        
        PhotonNetwork.SetPlayerCustomProperties(tagProperties);
        //Debug.Log("Tagger is: " + PhotonNetwork.PlayerList[rand].NickName);

    }


    [PunRPC]
    public void SetTaggerRPC(Player player)
    {
        if (player.IsLocal)
        {
            player.CustomProperties["tagStatus"] = true;

            tagProperties["tagStatus"] = player.CustomProperties["tagStatus"];

            PhotonNetwork.SetPlayerCustomProperties(tagProperties);

            Debug.Log(player.NickName + " is now a tagger");

        }
    }

    [PunRPC]
    public void SetFreeRPC(Player player)
    {
        if (player.IsLocal)
        {
            player.CustomProperties["tagStatus"] = false;

            tagProperties["tagStatus"] = player.CustomProperties["tagStatus"];

            PhotonNetwork.SetPlayerCustomProperties(tagProperties);

            Debug.Log(player.NickName + " is now free");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            //if (other.gameObject.CompareTag("Player"))
            //{
            //    if (other.gameObject.GetPhotonView().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            //    {
            //        ownership.ChangeOwner();
            //    }
            //}
            if (photonView.Owner.CustomProperties["tagStatus"] != null)
            {
                //Player tagger = photonView.Owner;

                if (other.gameObject.CompareTag("Player"))
                {
                    if ((bool)photonView.Owner.CustomProperties["tagStatus"])
                    {
                        Player player = other.gameObject.GetComponent<PhotonView>().Owner;

                        photonView.RPC("SetFreeRPC", RpcTarget.AllBuffered, photonView.Owner);

                        //Debug.Log("collision player tag status = " + (bool)player.CustomProperties["tagStatus"] + "\n (should be false)");

                        photonView.RPC("SetTaggerRPC", RpcTarget.AllBuffered, player);

                        Debug.Log("collision player tag status = " + (bool)player.CustomProperties["tagStatus"] + "\n (should be true)");

                        //PhotonNetwork.SetPlayerCustomProperties(tagProperties);
                    }
                }
            }
        }
    }
}
