using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class TagStatus : MonoBehaviourPunCallbacks
{

    public TextMeshProUGUI tagStatusText;

    [SerializeField] bool isTagger = false;

    ExitGames.Client.Photon.Hashtable tagProperties = new ExitGames.Client.Photon.Hashtable();


    private void Start()
    {
        tagProperties["tagStatus"] = false;
        isTagger = false;

        PhotonNetwork.SetPlayerCustomProperties(tagProperties);

        if (PhotonNetwork.IsMasterClient)
        {
            SetRandomTaggedPlayer();
        }

        //UpdateTagStatus(photonView.Owner);

        //UpdateTagStatus(PhotonNetwork.LocalPlayer);


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
       
        switch (player.CustomProperties["tagStatus"])
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

        //Debug.Log("[" + player.NickName + "] is the tagger? ... " + player.CustomProperties["tagStatus"] + " \n (0 = no, 1 = yes)");


        Debug.Log("status for '" + player.NickName + "' have been updated");

    }

    void SetRandomTaggedPlayer()
    {

        int rand = Random.Range(0, PhotonNetwork.PlayerList.Length);

        PhotonNetwork.PlayerList[rand].CustomProperties["tagStatus"] = true;

        tagProperties["tagStatus"] = PhotonNetwork.PlayerList[rand].CustomProperties["tagStatus"];

        PhotonNetwork.SetPlayerCustomProperties(tagProperties);

        if (photonView.Owner == PhotonNetwork.PlayerList[rand])
        {
            isTagger = true;
        }

        //Debug.Log("Tagger is: " + PhotonNetwork.PlayerList[rand].NickName);

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("Player") && isTagger)
        {
            other.gameObject.GetComponent<PhotonView>().Owner.CustomProperties["tagStatus"] = true;
            tagProperties["tagStatus"] = false;

            PhotonNetwork.SetPlayerCustomProperties(tagProperties);

            //UpdateTagStatus(other.gameObject.GetComponent<PhotonView>().Owner);
            //UpdateTagStatus(photonView.Owner);

            Debug.Log("Tagger has changed");
        }

        
    }

}
