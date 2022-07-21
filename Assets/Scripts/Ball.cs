using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toolbelt_OJ;

public class Ball : MonoBehaviour
{
    Rigidbody rb;

    public List<AudioClip> kickSounds;
    AudioSource audioSource;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rb.AddForce(other.gameObject.GetComponent<PlayerController>().moveDirection, ForceMode.Impulse);

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.PlayOneShot(kickSounds[Random.Range(0, kickSounds.Count)]);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime);
    //    rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.deltaTime);
    //}
}
