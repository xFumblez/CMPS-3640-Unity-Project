using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class IsItemGrabbed : MonoBehaviourPun
{
    public bool itemGrabbed;

    public void Start()
    {
        itemGrabbed = false;
    }

    [PunRPC]
    void OnItemGrab(bool isGrabbed)
    {
        itemGrabbed = isGrabbed;
    }
}
