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


    //public AudioClip teleSound;


    //private void Start()
    //{
    //    teleExits = new List<GameObject>();
    //    teleExits.AddRange(GameObject.FindGameObjectsWithTag("TeleExit"));


    //    for (int i = 0; i < teleExits.Count; i++)
    //    {
    //        if (teleExits[i] == gameObject)
    //        {
    //            teleExits.Remove(teleExits[i]);
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {

            //exitChoice = Random.Range(0, teleExits.Count);

        randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));


        //other.transform.position = teleExits[exitChoice].transform.position;

        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = randomPosition; //teleExits[exitChoice].transform.position;

            other.GetComponent<CharacterController>().enabled = true;


        }
        else
        {
            other.transform.position = randomPosition;
            //other.transform.position = teleExits[exitChoice].transform.position;
        }

            //other.GetComponent<PhotonView>().RPC("Teleport", RpcTarget.AllBuffered, other.gameObject, teleExits[exitChoice]);

        if (other.transform.position == randomPosition) //teleExits[exitChoice].transform.position
        {
            Debug.Log("teleported a player to " + randomPosition.ToString());
        }
        else
        {
            Debug.Log("failed to teleport");
        }
            //GetComponent<AudioSource>().PlayOneShot(teleSound);
            
        
        
    }

    
}
