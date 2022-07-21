using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Teleporter : MonoBehaviourPunCallbacks
{
    //public Transform teleExit;
    //public List<GameObject> teleExits;
    //int exitChoice;

    public float minX, maxX, minY, maxY, minZ, maxZ;

    Vector3 randomPosition;


    public AudioClip teleSound;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {



        randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));



        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = randomPosition; 

            other.GetComponent<CharacterController>().enabled = true;


        }
        else
        {
            other.transform.position = randomPosition;
 
        }


        if (other.transform.position == randomPosition)
        {
            Debug.Log("teleported a player to " + randomPosition.ToString());
        }
        else
        {
            Debug.Log("failed to teleport");
        }
            GetComponent<AudioSource>().PlayOneShot(teleSound);

       
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.PlayOneShot(teleSound);

    }

    
}
