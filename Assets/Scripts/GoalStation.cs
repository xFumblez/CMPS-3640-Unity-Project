using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GoalStation : MonoBehaviourPun
{
    public List<GameObject> requestedObjects;
    public List<string> receivedObjects;
    public List<GameObject> possibleObjects;
    [SerializeField] private PhotonView myView;
    [SerializeField] private PhotonView otherView;


    // Start is called before the first frame update
    void Start()
    {
        myView = GetComponent<PhotonView>();
        requestedObjects.Clear();
        receivedObjects.Clear();
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

            otherView = other.gameObject.GetComponent<PhotonView>();
            myView.RPC("AddToReceived", RpcTarget.All, other.gameObject.tag);

            DestroyObject(otherView);
        }
    }

    [PunRPC]
    void ClearObjectList(List<GameObject> objectList)
    {
        objectList.Clear();
    }

    [PunRPC]
    void AddToReceived(string itemToAdd)
    {
        receivedObjects.Add(itemToAdd);
        Debug.Log(itemToAdd + " was received at " + gameObject.name);
    }

    void DestroyObject(PhotonView otherView)
    {
        PhotonNetwork.Destroy(otherView);
    }

    [PunRPC]
    void requestObject(bool isEasy)
    {
        // Random number to determine what item is requested
        int randomNumber = Random.Range(0, 100);

        // Boolean isEasy to determine if basic item or enhanced item is requested
        if (isEasy)
        {
            if (randomNumber >= 0 && randomNumber < 33)
            {
                requestedObjects.Add(possibleObjects[0]);
            }
            else if (randomNumber >= 33 && randomNumber < 66)
            {
                requestedObjects.Add(possibleObjects[1]);
            }
            else if (randomNumber >= 66 && randomNumber < 99)
            {
                requestedObjects.Add(possibleObjects[2]);
            }
            else
            {
                requestedObjects.Add(possibleObjects[3]);
            }
        }
        else
        {
            if (randomNumber >= 0 && randomNumber < 33)
            {
                requestedObjects.Add(possibleObjects[4]);
            }
            else if (randomNumber >= 33 && randomNumber < 66)
            {
                requestedObjects.Add(possibleObjects[5]);
            }
            else if (randomNumber >= 66 && randomNumber < 99)
            {
                requestedObjects.Add(possibleObjects[6]);
            }
            else
            {
                requestedObjects.Add(possibleObjects[7]);
            }
        }
    }
}
