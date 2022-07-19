using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class NameTag : MonoBehaviourPun
{
    [SerializeField] private TextMeshProUGUI playerName;

    void Start()
    {
        //SetNameTag();
        if (photonView.IsMine)
        {
            //playerName.enabled = false;
            return;
        }
        else
        {
            SetNameTag();
        }
    }

    void SetNameTag() => playerName.text = photonView.Owner.NickName;

}
