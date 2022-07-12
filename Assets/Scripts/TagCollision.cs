using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TagCollision : MonoBehaviourPunCallbacks
{
    TagStatus tagStatus;


    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (photonView.Owner.CustomProperties["tagStatus"] != null)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    if ((bool)photonView.Owner.CustomProperties["tagStatus"])
                    {
                        Player player = other.gameObject.GetComponent<PhotonView>().Owner;

                        other.gameObject.GetComponent<PhotonView>().RPC("SetTagger", RpcTarget.All, player);

                        photonView.RPC("SetFree", RpcTarget.All, photonView.Owner);
                    }

                }
            }

        }
    }

}
