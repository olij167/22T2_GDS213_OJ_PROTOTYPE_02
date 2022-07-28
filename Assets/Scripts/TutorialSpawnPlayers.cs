using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class TutorialSpawnPlayers : MonoBehaviourPun
{
    public GameObject[] playerColours;

    public int playerCount;

    //public TextMeshProUGUI textBox;


    public float minX, maxX, minY, maxY, minZ, maxZ;

    private void Start()
    {
        playerCount = PhotonNetwork.PlayerList.Length;

        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));

        GameObject playerToSpawn = playerColours[Random.Range(0, playerColours.Length)];

        PhotonNetwork.Instantiate(playerToSpawn.name, randomPosition, Quaternion.identity);

        Cursor.lockState = CursorLockMode.Locked;

    }

}
