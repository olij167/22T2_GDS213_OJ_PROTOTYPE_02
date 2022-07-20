using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TagStatus : MonoBehaviourPunCallbacks
{
    PlayerStartPrompt startPrompt;

    public TextMeshProUGUI tagStatusText;

    Hashtable tagProperties = new Hashtable();

    public bool tagStatus = false, justTagged;

    public float tagCooldown;

    PauseMenu pauseMenu;

    private void Start()
    {
        //tagObject = GameObject.FindGameObjectWithTag("TagObject");
        //ownership = tagObject.GetComponent<OwnershipTransfer>();
        startPrompt = GameObject.FindGameObjectWithTag("RunText").GetComponent<PlayerStartPrompt>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenu>();

        tagProperties["tagStatus"] = tagStatus;

        if (PhotonNetwork.IsMasterClient)
        {
            //ownership.ChangeOwner();
            Player randomPlayer = PhotonNetwork.PlayerList[Random.Range(1, PhotonNetwork.PlayerList.Length)];

            SetTagger(randomPlayer);
        }

        PhotonNetwork.SetPlayerCustomProperties(tagProperties);

        startPrompt.SetText();
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
                    tagStatus = false;
                    tagStatusText.color = Color.green;
                    break;
                }

            case true:
                {
                    tagStatusText.text = "IT";
                    tagStatus = true;
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


    void SetTagger(Player player)
    {
        photonView.RPC("SetTaggerRPC", RpcTarget.AllBuffered, player);
    }


    [PunRPC]
    public void SetTaggerRPC(Player player)
    {
        if (player.IsLocal)
        {
            player.CustomProperties["tagStatus"] = true;

            tagProperties["tagStatus"] = player.CustomProperties["tagStatus"];

            startPrompt.SetText();

            PhotonNetwork.SetPlayerCustomProperties(tagProperties);

            Debug.Log(player.NickName + " is now a tagger");

            pauseMenu.UpdatePlayerListTagStatus();

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

                        this.Invoke(() => SetTagger(player), tagCooldown);

                        //photonView.RPC("SetTaggerRPC", RpcTarget.AllBuffered, player);

                        Debug.Log("collision player tag status = " + (bool)player.CustomProperties["tagStatus"] + "\n (should be true)");

                        startPrompt.SetText();
                        //photonView.RPC("SetText", RpcTarget.AllBuffered);

                        photonView.RPC("TagImpactEffect", RpcTarget.All);

                        //PhotonNetwork.SetPlayerCustomProperties(tagProperties);
                    }
                }
            }
        }

        // play tag sound effect

    }

    [PunRPC]
    public void TagImpactEffect()
    {
        startPrompt.displayImage = true;
    }

}
    public static class Utility
    {
        public static void Invoke(this MonoBehaviour mb, System.Action f, float delay)
        {
            mb.StartCoroutine(InvokeRoutine(f, delay));
        }

        private static IEnumerator InvokeRoutine(System.Action f, float delay)
        {
            yield return new WaitForSeconds(delay);
            f();
        }
    }
