using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnItem : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject spawnableObject;
    private Vector3 spawnPosoffset;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnObject()
    {
        spawnPosoffset = new Vector3(Random.Range(0, 1.5f), 0, Random.Range(0, 1.5f));
        PhotonNetwork.Instantiate(spawnableObject.name, spawnPos.position + spawnPosoffset, Quaternion.identity);
    }
}
