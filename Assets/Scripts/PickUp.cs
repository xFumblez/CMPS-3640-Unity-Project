using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PickUp : MonoBehaviour
{
    public float pickUpRange = 10f;
    public float moveForce = 250f;
    public float throwForce = 600f;
    public Transform holdParent;
    private GameObject heldObject;
    private GameObject objRef;
    PhotonView playerView, itemView;
    public Animator playerAnim;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerView.IsMine)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {

                if (heldObject == null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, pickUpRange))
                    {
                        if (hit.transform.GetComponent<PhotonView>() == null)
                            return;
                        else
                        {
                            itemView = hit.transform.GetComponent<PhotonView>();
                        }

                        if (hit.transform.gameObject.GetComponent<IsItemGrabbed>() != null && hit.transform.gameObject.GetComponent<IsItemGrabbed>().itemGrabbed == false)
                        {
                            itemView.TransferOwnership(PhotonNetwork.LocalPlayer);
                            hit.transform.gameObject.GetComponent<IsItemGrabbed>().photonView.RPC("OnItemGrab", RpcTarget.All, true);
                            objRef = hit.transform.gameObject;
                            PickupObject(objRef);
                        }
                        else
                            return;
                    }
                }
                else
                {
                    DropObject();
                }
            }

            if (heldObject != null)
            {
                MoveObject();

                if (Input.GetKeyDown(KeyCode.R))
                {
                    ThrowObject();
                }
            }
        }
    }

    void ThrowObject()
    {
        heldObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
        DropObject();
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObject.transform.position, holdParent.position) > 0.1f)
        {
            Vector3 moveDirection = (holdParent.position - heldObject.transform.position);
            heldObject.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }

    void PickupObject(GameObject pickObj)
    {

        if (pickObj.GetComponent<Rigidbody>())
        {
            Rigidbody objRig = pickObj.GetComponent<Rigidbody>();
            objRig.useGravity = false;
            objRig.drag = 10;

            objRig.transform.parent = holdParent;
            heldObject = pickObj;
            playerAnim.SetBool("itemHeld", true);
        }
    }

    void DropObject()
    {
        Rigidbody heldRig = heldObject.GetComponent<Rigidbody>();
        heldObject.GetComponent<Rigidbody>().useGravity = true;
        heldRig.drag = 1;

        heldObject.GetComponent<IsItemGrabbed>().photonView.RPC("OnItemGrab", RpcTarget.All, false);
        heldObject.transform.parent = null;
        heldObject = null;
        playerAnim.SetBool("itemHeld", false);
    }
}
