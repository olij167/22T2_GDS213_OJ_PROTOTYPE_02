using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[System.Serializable]
public class TagStatus : MonoBehaviourPunCallbacks
{
    PlayerStartPrompt startPrompt;

    public TextMeshProUGUI tagStatusText;

    Hashtable tagProperties = new Hashtable();

    public bool tagStatus = false;

    public float tagCooldown;

    private GameTimer gameTimer;

    private List<Player> currentTaggers;
    public List<GameObject> currentTaggerGameObjects;

    //public AudioClip tagClip;
    //AudioSource tagSource;

    private void Start()
    {
        
        
        gameTimer = GameObject.FindGameObjectWithTag("Timer").GetComponent<GameTimer>();

        startPrompt = GameObject.FindGameObjectWithTag("RunText").GetComponent<PlayerStartPrompt>();
        

        tagProperties["tagStatus"] = true;
        

        //foreach(Player player in PhotonNetwork.PlayerList)
        //{
        //    player.SetCustomProperties(tagProperties);
        //}

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("I am the master");

            photonView.RPC("SetFreeRPC", RpcTarget.All, photonView.Owner);

            PhotonNetwork.SetPlayerCustomProperties(tagProperties);
        }
        else
        {
            Debug.Log("I am a client");
        }

        //gameTimer.tagger = gameTimer.GetTagger();
        //tagSource = GameObject.FindGameObjectWithTag("TagUI").GetComponent<AudioSource>();
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
                    tagStatus = false;
                    tagStatusText.color = Color.green;

                    if (photonView.IsMine)
                    {
                        startPrompt.SetText();
                    }
                    break;
                }

            case true:
                {
                    tagStatusText.text = "IT";
                    tagStatus = true;
                    tagStatusText.color = Color.red;
                    if (photonView.IsMine)
                    {
                        startPrompt.SetText();
                    }
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

    [PunRPC]
    void SetRandomTagger(Player randomPlayer)
    {


        Debug.Log("Player List Length = " + PhotonNetwork.PlayerList.Length + "\n tagger = " + randomPlayer.NickName);

        randomPlayer.CustomProperties["tagStatus"] = true;

        ////if (randomPlayer.IsLocal)
        //{
        //    tagProperties["tagStatus"] = randomPlayer.CustomProperties["tagStatus"];
        //}
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            tagProperties["tagStatus"] = player.CustomProperties["tagStatus"];
            

        }

        PhotonNetwork.SetPlayerCustomProperties(tagProperties);


    }

    //[PunRPC]
    //public void CheckCurrentTagger()
    //{
    //    Debug.Log("Checking Number of Taggers");

    //    //currentTaggers.Clear();
    //    currentTaggers = new List<Player>();


    //    for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
    //    {
    //        if (PhotonNetwork.PlayerList[i].CustomProperties["tagStatus"] != null)
    //        {
    //            if ((bool)PhotonNetwork.PlayerList[i].CustomProperties["tagStatus"])
    //            {
    //                //return PhotonNetwork.PlayerList[i];
    //                //taggerCount += 1;
    //                currentTaggers.Add(PhotonNetwork.PlayerList[i]);
    //                Debug.Log(PhotonNetwork.PlayerList[i].NickName + " added to current taggers list");

    //            }
    //        }
    //    }

    //    if (currentTaggers.Count > 1)
    //    {
    //        Debug.Log("Too many taggers, trying to free the surplus");

    //        for (int x = 0; x < currentTaggers.Count; x++)
    //        {
    //            if (currentTaggers[x] != currentTaggers[0])
    //            {
    //                photonView.RPC("SetFreeRPC", RpcTarget.AllBuffered, currentTaggers[x]);
    //            }
    //        }

    //        //photonView.RPC("CheckCurrentTagger", RpcTarget.AllBuffered);
    //    }
    //}

    void SetTagger(Player player)
    {
        photonView.RPC("SetTaggerRPC", RpcTarget.All, player);

    }


    [PunRPC]
    public void SetTaggerRPC(Player player)
    {
        if (player.IsLocal)
        {
            //Debug.Log("Setting new Tagger");

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
    
    //[PunRPC]
    //public void SetAllFreeRPC()
    //{
    //    foreach(Player player in PhotonNetwork.PlayerList)
    //    {
    //        player.CustomProperties["tagStatus"] = false;

    //        tagProperties["tagStatus"] = player.CustomProperties["tagStatus"];

    //        Debug.Log(player.NickName + " is now free");
    //    }

    //    PhotonNetwork.SetPlayerCustomProperties(tagProperties);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine && !gameTimer.gameOver)
        {
            if (photonView.Owner.CustomProperties["tagStatus"] != null)
            {
                //Player tagger = photonView.Owner;

                if (other.gameObject.CompareTag("Player"))
                {
                    if ((bool)photonView.Owner.CustomProperties["tagStatus"])
                    {
                        Player player = other.gameObject.GetComponent<PhotonView>().Owner;

                        if (!gameTimer.isBuildUps)
                        {
                            photonView.RPC("SetFreeRPC", RpcTarget.All, photonView.Owner);
                        }

                        //Debug.Log("collision player tag status = " + (bool)player.CustomProperties["tagStatus"] + "\n (should be false)");

                        this.Invoke(() => SetTagger(player), tagCooldown);

                        //photonView.RPC("SetTaggerRPC", RpcTarget.AllBuffered, player);

                        Debug.Log("collision player tag status = " + (bool)player.CustomProperties["tagStatus"] + "\n (should be true)");

                        //startPrompt.SetText();
                        //photonView.RPC("SetText", RpcTarget.AllBuffered);

                        photonView.RPC("TagImpactEffect", RpcTarget.All);

                        gameTimer.tagger = gameTimer.GetTagger();

                        //tagSource.Play();

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
        if (startPrompt != null)
        {
            startPrompt.displayImage = true;
        }
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
