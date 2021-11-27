using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;
    private int playerSpawnValue;
    public GameObject itemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        int randomNumber = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomNumber];

        if (PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == null)
        {
            playerSpawnValue = 0;
        }
        else
        {
            playerSpawnValue = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"];
        }

        GameObject playerToSpawn = playerPrefabs[playerSpawnValue];
        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(itemPrefab.name, Vector3.up, Quaternion.identity);
    }

}
