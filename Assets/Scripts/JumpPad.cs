using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolbelt_OJ;

public class JumpPad : MonoBehaviour
{
    public float jumpForce;

    //public List<AudioClip> trampolineAudio;
    //private AudioSource audioSource;
    public void OnTriggerEnter(Collider other)
    {
        //other.GetComponent<CharacterController>().

        if(other.GetComponent<Rigidbody>())
        {
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().moveDirection.y = jumpForce;


        }

        //audioSource.PlayOneShot(trampolineAudio[Random.Range(0, trampolineAudio.Count)]);
    }
}
