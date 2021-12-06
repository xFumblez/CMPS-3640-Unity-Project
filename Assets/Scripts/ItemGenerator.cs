using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemGenerator : MonoBehaviour
{
    public float interactRange = 2f;
    PhotonView playerView, itemView;
    private SpawnItem spawnItemScript;

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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, interactRange))
                {
                    if (hit.transform.GetComponent<PhotonView>() == null)
                        return;
                    else
                    {
                        itemView = hit.transform.GetComponent<PhotonView>();
                        if (itemView.transform.gameObject.tag == "Sword Gen")
                        {
                            spawnItemScript = itemView.gameObject.GetComponent<SpawnItem>();
                            spawnItemScript.SpawnObject();
                        }
                        else if (itemView.transform.gameObject.tag == "Axe Gen")
                        {
                            spawnItemScript = itemView.gameObject.GetComponent<SpawnItem>();
                            spawnItemScript.SpawnObject();
                        }
                        else if (itemView.transform.gameObject.tag == "Shield Gen")
                        {
                            spawnItemScript = itemView.gameObject.GetComponent<SpawnItem>();
                            spawnItemScript.SpawnObject();
                        }
                        else if (itemView.transform.gameObject.tag == "Bow Gen")
                        {
                            spawnItemScript = itemView.gameObject.GetComponent<SpawnItem>();
                            spawnItemScript.SpawnObject();
                        }
                        else
                            return;
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }
}
