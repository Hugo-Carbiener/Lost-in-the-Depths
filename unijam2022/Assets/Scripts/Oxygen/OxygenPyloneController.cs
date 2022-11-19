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
    public bool isActivePylone;

    [Header("Network integration")]
    [SerializeField] private GameObject prevPylone; //reference towards previous pylone in network
    [SerializeField] private GameObject nextPylone; //reference towards next pylone in network
    [SerializeField] private GameObject networkConnection;

    [Header("Line Renderers")]
    private LineRenderer networkLineRenderer;
    private LineRenderer lineRenderer;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
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

        if (isActivePylone) //if the pylone is currently the one holding the player, we trigger the visual line with it
        {
            ConnectToPlayer();
        }
        else
        {
            DisconnectFromPlayer();
        }
    }

    public bool TestPlayerConnection()
    {
        //We now handle the player's detection :
        curDistance = (player.transform.position - transform.position).magnitude;
        if (curDistance < maxDistance)
        {
            return true;
        }
        return false;
    }

    

    /**
     *  Function stopping the oxygen flow
     */
    public void DisconnectFromPlayer()
    {
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
