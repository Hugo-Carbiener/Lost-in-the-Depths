using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

/**
 *  Component used by oxygen pylones
 */
public class OxygenPyloneController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private OxygenModuleController oxygenController;

    [SerializeField] private float maxDistance;
    private float curDistance;
    private bool isConnectedPlayer;

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
    }

    private void Update()
    {
        curDistance = (player.transform.position-transform.position).magnitude;
        Debug.Log("DISTANCE WITH PLAYER : " + curDistance);
        if (curDistance < maxDistance)
        {
            Debug.Log("CONNECTED TO OXYGEN NETWORK");
            isConnectedPlayer = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, player.transform.position);
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

    private void StartOxygenFlow()
    {
        oxygenController.isRecharging = true;
        StartCoroutine(oxygenController.AddOxygen(oxygenController.consumptionRate));
    }

    private void StopOxygenFlow()
    {
        oxygenController.isRecharging = false;
        lineRenderer.positionCount = 0;
    }

}
