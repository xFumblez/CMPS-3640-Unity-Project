using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPun
{
    public GameObject[] goalStations;
    bool startTimer = false;
    public double timerIncrementValue;
    public double startTime;
    public double timer = 300;
    private double requestObjectsTimeValue;
    private double currentTime;
    private double timeToWaitForRequest = 5;
    private int points = 0;

    // Start is called before the first frame update
    void Start()
    {
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
            foreach (GameObject item in goalStations)
            {
                GoalStation goalStation = item.GetComponentInChildren<GoalStation>();
                if (goalStation.getPoint)
                {
                    points++;
                    goalStation.myView.RPC("SetPointToFalse", RpcTarget.All);
                    Debug.Log(points);
                }
            }

            requestObjectsTimeValue = PhotonNetwork.Time - currentTime;

            if (requestObjectsTimeValue >= timeToWaitForRequest)
            {
                foreach (GameObject item in goalStations)
                {
                    int[] randomNumber = { Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 4) };
                    GoalStation goalStation = item.GetComponentInChildren<GoalStation>();
                    goalStation.myView.RPC("GetRequestCount", RpcTarget.All);
                    if (goalStation.reqCount < 3)
                    {
                        goalStation.myView.RPC("RequestObjects", RpcTarget.All, true, randomNumber);
                        goalStation.myView.RPC("DisplayRequestedObjects", RpcTarget.All);
                    }
                }
                currentTime = PhotonNetwork.Time;
                requestObjectsTimeValue = 0;
                timeToWaitForRequest = 15;
            }
        }

        if (timerIncrementValue >= timer)
        {
            Debug.Log("Time's Up!");
            startTimer = false;
        }
    }
}
