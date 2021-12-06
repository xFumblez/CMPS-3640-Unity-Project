using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.Animations;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    PhotonView view;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public TextMesh nameText;

    public Animator playerAnim;
    private Vector3 lastPos = new Vector3(0, 0, 0);
    public GameManager gameManager;
    public GameObject[] characterHide;

    bool isGrounded;

    Vector3 velocity;
    // Update is called once per frame
    void Start()
    {
        view = GetComponent<PhotonView>();
        nameText.text = view.Owner.NickName;
        gameManager = FindObjectOfType<GameManager>();

        if (view.IsMine)
        {
            for (int i = 0; i < characterHide.Length; i++)
            {
                characterHide[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        if(view.IsMine && Cursor.lockState == CursorLockMode.Locked)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);

            if(isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight*-2*gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            if (lastPos != gameObject.transform.position)
            {
                playerAnim.SetBool("isMoving", true);
            }
            else
            {
                playerAnim.SetBool("isMoving", false);
            }
            lastPos = gameObject.transform.position;

            if (gameManager.timerIncrementValue >= gameManager.timer / 2)
            {
                playerAnim.SetBool("isFrantic", true);
            }
            else
            {
                playerAnim.SetBool("isFrantic", false);
            }
        }
    }
}
