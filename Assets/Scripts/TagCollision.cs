using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TagCollision : MonoBehaviourPunCallbacks
{
    private TagStatus tagStatus;
    void Start()
    {
        tagStatus = GetComponent<TagStatus>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.name.Contains("Player") && (bool)PhotonNetwork.LocalPlayer.CustomProperties["tagStatus"])
            {
                other.gameObject.GetComponent<PhotonView>().Owner.CustomProperties["tagStatus"] = true;
                photonView.Owner.CustomProperties["tagStatus"] = false;

                tagStatus.UpdateTagStatus(other.gameObject.GetComponent<PhotonView>().Owner);
                tagStatus.UpdateTagStatus(photonView.Owner);

                Debug.Log("Tagger has changed");
            }

        }
    }
}
