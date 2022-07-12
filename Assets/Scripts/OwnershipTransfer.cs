using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OwnershipTransfer : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != base.photonView) return;

        //Add checks here

        base.photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (targetView != base.photonView) return;
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        if (targetView != base.photonView) return;
    }

    public void ChangeOwner()
    {
        base.photonView.RequestOwnership();
    }

    //private void OnOwnershipChange()
    //{
    //    if (transform.GetChild(4) == gameObject.CompareTag("TagObject"))
    //    {
    //        tagProperties["tagStatus"] = true;
    //        PhotonNetwork.SetPlayerCustomProperties(tagProperties);
    //    }
    //    else
    //    {
    //        tagProperties["tagStatus"] = false;
    //        PhotonNetwork.SetPlayerCustomProperties(tagProperties);
    //    }

    //}
}
