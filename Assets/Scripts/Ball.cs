using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolbelt_OJ;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rb.AddForce(other.gameObject.GetComponent<PlayerController>().moveDirection, ForceMode.Impulse);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime);
    //    rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.deltaTime);
    //}
}
