using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Billboard : MonoBehaviour
{
    private Transform vCamTransform;

    private void Start()
    {
        if(GameObject.FindGameObjectWithTag("vCam"))
        {
            vCamTransform = GameObject.FindGameObjectWithTag("vCam").transform;
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + vCamTransform.rotation * Vector3.forward, vCamTransform.rotation * Vector3.up);
    }
}
