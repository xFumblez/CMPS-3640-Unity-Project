using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    [SerializeField] private Camera cam1;

    PhotonView view;

    public Transform playerBody;

    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        view = GetComponent<PhotonView>();
        if(!view.IsMine)
        {
            cam1.enabled = false;
            cam1.GetComponent<AudioListener> ().enabled  =  false;
        }
        else
        {
            cam1.enabled = true;
            cam1.GetComponent<AudioListener> ().enabled  =  true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(view.IsMine)
        {
            float MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= MouseY;
            xRotation = Mathf.Clamp(xRotation,-90,90);

            transform.localRotation = Quaternion.Euler(xRotation,0f,0f);

            playerBody.Rotate(Vector3.up * MouseX);
        }
    }
}
