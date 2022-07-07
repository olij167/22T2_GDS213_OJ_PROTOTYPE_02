using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCamTransform;

    private void Start()
    {
        mainCamTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamTransform.rotation * Vector3.forward, mainCamTransform.rotation * Vector3.up);
    }
}
