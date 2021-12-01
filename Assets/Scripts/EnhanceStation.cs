using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class EnhanceStation : MonoBehaviourPun
{
    public string receivedObject;
    public List<GameObject> possibleObjects;
    [SerializeField] private PhotonView myView;
    [SerializeField] private PhotonView otherView;
    public Transform spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        myView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PhotonView>() == null)
            return;
        else
        {
            if (other.gameObject.tag == "Untagged" || other.gameObject.tag == "Player")
                return;

            if (gameObject.tag == "Sword Station")
            {
                otherView = other.gameObject.GetComponent<PhotonView>();
                myView.RPC("SwordStationAdd", RpcTarget.All, other.gameObject.tag);
            }

            else if (gameObject.tag == "Axe Station")
            {
                otherView = other.gameObject.GetComponent<PhotonView>();
                myView.RPC("AxeStationAdd", RpcTarget.All, other.gameObject.tag);
            }

            else if (gameObject.tag == "Shield Station")
            {
                otherView = other.gameObject.GetComponent<PhotonView>();
                myView.RPC("ShieldStationAdd", RpcTarget.All, other.gameObject.tag);
            }

            else if (gameObject.tag == "Bow Station")
            {
                otherView = other.gameObject.GetComponent<PhotonView>();
                myView.RPC("BowStationAdd", RpcTarget.All, other.gameObject.tag);
            }

            if (otherView == null || receivedObject == null)
                return;

            DestroyObject(otherView);

            CreateObject(receivedObject);
        }
    }

    [PunRPC]
    void ClearObjectList(List<GameObject> objectList)
    {
        objectList.Clear();
    }

    [PunRPC]
    void SwordStationAdd(string itemToAdd)
    {
        if (itemToAdd != "Basic Sword")
        {
            receivedObject = null;
            return;
        }

        receivedObject = itemToAdd;
        Debug.Log(itemToAdd + " was received at " + gameObject.tag);
    }

    [PunRPC]
    void AxeStationAdd(string itemToAdd)
    {
        if (itemToAdd != "Basic Axe")
        {
            receivedObject = null;
            return;
        }
        receivedObject = itemToAdd;
        Debug.Log(itemToAdd + " was received at " + gameObject.tag);
    }

    [PunRPC]
    void ShieldStationAdd(string itemToAdd)
    {
        if (itemToAdd != "Basic Shield")
        {
            receivedObject = null;
            return;
        }

        receivedObject = itemToAdd;
        Debug.Log(itemToAdd + " was received at " + gameObject.tag);
    }

    [PunRPC]
    void BowStationAdd(string itemToAdd)
    {
        if (itemToAdd != "Basic Bow")
        {
            receivedObject = null;
            return;
        }

        receivedObject = itemToAdd;
        Debug.Log(itemToAdd + " was received at " + gameObject.tag);
    }

    void DestroyObject(PhotonView otherView)
    {
        PhotonNetwork.Destroy(otherView);
    }

    void CreateObject(string objectToSpawn)
    {
        GameObject objSpawn = null;

        if (objectToSpawn == "Basic Sword")
        {
            objSpawn = possibleObjects[0];
        }
        else if (objectToSpawn == "Basic Axe")
        {
            objSpawn = possibleObjects[1];
        }
        else if (objectToSpawn == "Basic Shield")
        {
            objSpawn = possibleObjects[2];
        }
        else if (objectToSpawn == "Basic Bow")
        {
            objSpawn = possibleObjects[3];
        }

        if (objSpawn == null)
            return;

        PhotonNetwork.Instantiate(objSpawn.name, spawnPos.position, Quaternion.identity);
    }
}
