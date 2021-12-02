using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PauseMenuFunctions : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuUI;
    public bool Paused = false;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!Paused)
            {
                Paused = !Paused;
                PauseMenuUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Paused = !Paused;
                PauseMenuUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    public void Resume()
    {
        Paused = false;
        PauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void QuitLobby()
    {
        PauseMenuUI.SetActive(false);
        Paused = false;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Lobby");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
