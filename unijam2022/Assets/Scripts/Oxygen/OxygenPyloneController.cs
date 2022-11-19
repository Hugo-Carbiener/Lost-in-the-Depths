using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

/**
 *  Component used by oxygen pylones
 */
public class OxygenPyloneController : MonoBehaviour
{
    [Header("Player data")]
    [SerializeField] private GameObject player;
    private OxygenModuleController oxygenController; //ref to player's oxygen module

    [Header("Distance data")]
    [SerializeField] private float maxDistance;
    private float curDistance;
    private bool isConnectedPlayer;

    [Header("Network integration")]
    [SerializeField] private GameObject prevPylone; //reference towards previous pylone in network
    [SerializeField] private GameObject nextPylone; //reference towards next pylone in network
    [SerializeField] private GameObject networkConnection;

    [Header("Line Renderers")]
    private LineRenderer networkLineRenderer;
    private LineRenderer lineRenderer;

    private void Start()
    {
        oxygenController = player.GetComponent<OxygenModuleController>();
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            transform.AddComponent<LineRenderer>();
            lineRenderer = GetComponent<LineRenderer>();
        }
        isConnectedPlayer = false;
        networkLineRenderer = networkConnection.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        //if the previous Pylone exists, we connect this pylone to it ==> SHOULD BE CALLED ONLY WHEN NETWORK CHANGES RATHER THAN EVERY FRAME
        if (prevPylone != null)
        {
            prevPylone.GetComponent<OxygenPyloneController>().ConnectToPylone(gameObject);
        }

        //We now handle the player's detection :
        curDistance = (player.transform.position-transform.position).magnitude;
        Debug.Log("DISTANCE WITH PLAYER : " + curDistance);
        if (curDistance < maxDistance)
        {
            Debug.Log("CONNECTED TO OXYGEN NETWORK");
            isConnectedPlayer = true;
            ConnectToPlayer();
            if (!oxygenController.isRecharging)
            {
                StartOxygenFlow();
            }
        }
        else
        {
            Debug.Log("NO LONGER CONNECTED TO OXYGEN NETWORK");
            isConnectedPlayer = false;
            StopOxygenFlow();
        }
    }

    /**
     *  Function starting the oxygen flow towards the player
     */
    private void StartOxygenFlow()
    {
        oxygenController.isRecharging = true;
        StartCoroutine(oxygenController.AddOxygen(oxygenController.consumptionRate));
    }

    /**
     *  Function stopping the oxygen flow
     */
    private void StopOxygenFlow()
    {
        oxygenController.isRecharging = false;
        lineRenderer.positionCount = 0;
    }

    /**
     *  Function that will visually connect to an inputted pylone
     */
    public void ConnectToPylone(GameObject connectPylone)
    {
        networkLineRenderer.positionCount = 2;
        networkLineRenderer.SetPosition(0, transform.position);
        networkLineRenderer.SetPosition(1, connectPylone.transform.position);
    }

    /**
     *  Function that will visually connect to player
     */
    private void ConnectToPlayer()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, player.transform.position);
    }

}
