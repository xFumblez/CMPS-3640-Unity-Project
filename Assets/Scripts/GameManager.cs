using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPun
{
    public GameObject[] goalStations;
    public Text timerText;
    public Text pointText;
    bool startTimer = false;
    public double timerIncrementValue;
    public double startTime;
    public double timer = 300;
    private double requestObjectsTimeValue;
    private double currentTime;
    private double timeToWaitForRequest = 5;
    private int points = 0;
    public PhotonView thisView;

    // Start is called before the first frame update
    void Start()
    {
        thisView = GetComponent<PhotonView>();
        startTime = PhotonNetwork.Time;
        currentTime = startTime;
        startTimer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startTimer)
            return;

        timerIncrementValue = PhotonNetwork.Time - startTime;

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            UpdatePoints();
            RequestObjects();
        }

        UpdateTimer((float)timer - (float)timerIncrementValue);

        if (timerIncrementValue >= timer)
        {
            Debug.Log("Time's Up!");
            startTimer = false;
        }
    }

    void UpdatePoints()
    {
        foreach (GameObject item in goalStations)
        {
            GoalStation goalStation = item.GetComponentInChildren<GoalStation>();
            if (goalStation.getPoint)
            {
                points++;
                goalStation.myView.RPC("SetPoint", RpcTarget.All, false);
                thisView.RPC("SendPoints", RpcTarget.All, points);
            }
        }
    }

    [PunRPC]
    void SendPoints(int points)
    {
        pointText.text = "Points: " + points;
    }

    void RequestObjects()
    {
        requestObjectsTimeValue = PhotonNetwork.Time - currentTime;

        if (requestObjectsTimeValue >= timeToWaitForRequest)
        {
            foreach (GameObject item in goalStations)
            {
                GoalStation goalStation = item.GetComponentInChildren<GoalStation>();
                goalStation.myView.RPC("GetRequestCount", RpcTarget.All);
                if (goalStation.reqCount < 3)
                {
                    if (timerIncrementValue >= timer / 2)
                    {
                        int[] randomNumber = { Random.Range(0, 8), Random.Range(0, 8), Random.Range(0, 8) };
                        goalStation.myView.RPC("RequestObjects", RpcTarget.All, randomNumber);
                        goalStation.myView.RPC("DisplayRequestedObjects", RpcTarget.All);
                    }
                    else
                    {
                        int[] randomNumber = { Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4) };
                        goalStation.myView.RPC("RequestObjects", RpcTarget.All, randomNumber);
                        goalStation.myView.RPC("DisplayRequestedObjects", RpcTarget.All);
                    }
                }
            }
            currentTime = PhotonNetwork.Time;
            requestObjectsTimeValue = 0;
            timeToWaitForRequest = 15;
        }
    }

    void UpdateTimer(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
