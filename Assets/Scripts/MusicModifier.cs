using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicModifier : MonoBehaviour
{
    public AudioSource gameAudio;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameAudio = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.timer - gameManager.timerIncrementValue <= 15)
        {
            gameAudio.pitch = 1.8f;
        }
        else if (gameManager.timerIncrementValue >= gameManager.timer / 1.25 && gameManager.timer - gameManager.timerIncrementValue > 15)
        {
            gameAudio.pitch = 1.5f;
        }
        else if (gameManager.timerIncrementValue >= gameManager.timer / 2 && gameManager.timerIncrementValue < gameManager.timer / 1.25)
        {
            gameAudio.pitch = 1.3f;
        }
        else if (gameManager.timerIncrementValue >= gameManager.timer / 4 && gameManager.timerIncrementValue < gameManager.timer / 2)
        {
            gameAudio.pitch = 1.1f;
        }
        else
        {
            gameAudio.pitch = 1;
        }
    }
}
