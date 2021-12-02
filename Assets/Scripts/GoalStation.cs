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
    public PhotonView myView;
    public TextMesh[] requestDisplays;
    public int reqCount = 0;
    [SerializeField] private PhotonView otherView;
    public bool timeToClear = false;
    public bool getPoint = false;


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
        CheckRequestedObjects();

        if (timeToClear)
        {
            myView.RPC("ClearObjectLists", RpcTarget.All);
            timeToClear = false;
        }
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

    void CheckRequestedObjects()
    {
        int count = 0;

        for (int i = 0; i < receivedObjects.Count; i++)
        {
            for (int j = 0; j < requestedObjects.Count; j++)
            {
                if (requestedObjects[j].tag == receivedObjects[i])
                {
                    count++;
                }
            }
        }

        if (count == 3)
        {
            timeToClear = true;
            myView.RPC("SetPoint", RpcTarget.All, true);
            myView.RPC("ChangeDisplay", RpcTarget.All, "Sweet Thanks!");
        }
        if (count != 3 && receivedObjects.Count == 3)
        {
            timeToClear = true;
            myView.RPC("ChangeDisplay", RpcTarget.All, "I didn't order these...");
        }
    }

    [PunRPC]
    void ChangeDisplay(string dialogue)
    {
        requestDisplays[0].text = "";
        requestDisplays[1].text = dialogue;
        requestDisplays[2].text = "";
    }

    [PunRPC]
    void DisplayRequestedObjects()
    {
        for (int i = 0; i < requestedObjects.Count; i++)
        {
            requestDisplays[i].text = requestedObjects[i].gameObject.tag;
        }
    }

    [PunRPC]
    void ClearObjectLists()
    {
        receivedObjects.Clear();
        receivedObjects.TrimExcess();
        requestedObjects.Clear();
        requestedObjects.TrimExcess();
        reqCount = 0;
    }

    [PunRPC]
    void AddToReceived(string itemToAdd)
    {
        receivedObjects.Add(itemToAdd);
        for (int i = 0; i < requestDisplays.Length; i++)
        {
            if (itemToAdd == requestDisplays[i].text)
            {
                requestDisplays[i].text = "";
                break;
            }
        }
        Debug.Log(itemToAdd + " was received at " + gameObject.name);
    }

    void DestroyObject(PhotonView otherView)
    {
        PhotonNetwork.Destroy(otherView);
    }

    [PunRPC]
    void GetRequestCount()
    {
        reqCount = requestedObjects.Count;
    }

    [PunRPC]
    void SetPoint(bool value)
    {
        getPoint = value;
    }

    [PunRPC]
    void RequestObjects(bool isEasy, int[] randomNumbers)
    {

        // Boolean isEasy to determine if basic item or enhanced item is requested
        if (isEasy)
        {
            for (int i = 0; i < 3; i++)
            {
                requestedObjects.Add(possibleObjects[randomNumbers[i]]);
            }

            /*if (randomNumber >= 0 && randomNumber < 33)
            {
                requestedObjects.Add(possibleObjects[0]);
                requestedObjects.Add(possibleObjects[1]);
                requestedObjects.Add(possibleObjects[2]);
            }
            else if (randomNumber >= 33 && randomNumber < 66)
            {
                requestedObjects.Add(possibleObjects[1]);
                requestedObjects.Add(possibleObjects[2]);
                requestedObjects.Add(possibleObjects[3]);
            }
            else if (randomNumber >= 66 && randomNumber < 99)
            {
                requestedObjects.Add(possibleObjects[0]);
                requestedObjects.Add(possibleObjects[2]);
                requestedObjects.Add(possibleObjects[3]);
            }
            else
            {
                requestedObjects.Add(possibleObjects[0]);
                requestedObjects.Add(possibleObjects[2]);
                requestedObjects.Add(possibleObjects[4]);
            }*/
        }
        else
        {
            /*if (randomNumber >= 0 && randomNumber < 33)
            {
                requestedObjects.Add(possibleObjects[4]);
                requestedObjects.Add(possibleObjects[7]);
                requestedObjects.Add(possibleObjects[5]);
            }
            else if (randomNumber >= 33 && randomNumber < 66)
            {
                requestedObjects.Add(possibleObjects[5]);
                requestedObjects.Add(possibleObjects[3]);
                requestedObjects.Add(possibleObjects[6]);
            }
            else if (randomNumber >= 66 && randomNumber < 99)
            {
                requestedObjects.Add(possibleObjects[6]);
                requestedObjects.Add(possibleObjects[7]);
                requestedObjects.Add(possibleObjects[4]);
            }
            else
            {
                requestedObjects.Add(possibleObjects[7]);
                requestedObjects.Add(possibleObjects[0]);
                requestedObjects.Add(possibleObjects[5]);
            }*/
        }
    }
}
