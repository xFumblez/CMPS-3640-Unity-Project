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
            if (requestDisplays[i].text == "")
                count++;
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
        bool found = false;
        receivedObjects.Add(itemToAdd);
        for (int i = 0; i < requestDisplays.Length; i++)
        {
            if (itemToAdd == requestDisplays[i].text)
            {
                requestDisplays[i].text = "";
                found = true;
                break;
            }
        }
        if (!found)
        {
            timeToClear = true;
            ChangeDisplay("I didn't order this...");
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
    void RequestObjects(int[] randomNumbers)
    {
        for (int i = 0; i < 3; i++)
        {
            requestedObjects.Add(possibleObjects[randomNumbers[i]]);
        }
    }
        
}
