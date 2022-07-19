using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public List<AudioClip> rustlingBushAudio;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(rustlingBushAudio[Random.Range(0, rustlingBushAudio.Count)]);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(rustlingBushAudio[Random.Range(0, rustlingBushAudio.Count)]);
        }
    }
}
