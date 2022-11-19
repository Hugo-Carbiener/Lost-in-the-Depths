using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/**
 *  Component used by the Oxygen network to allow for optimisation
 *  Placed at the pump that will act as the root of the system
 */
public class OxygenNetwork : MonoBehaviour
{
    [SerializeField] private GameObject[] pylonesNetwork;
    private GameObject curPylone;

    [Header("Player data")]
    [SerializeField] private GameObject player;
    private OxygenModuleController oxygenController; //ref to player's oxygen module

    [Header("Oxygen data")]
    [SerializeField] private float pumpOxygenRate;
    private float curOxygenRate;

    private void Start()
    {
        curOxygenRate = pumpOxygenRate;
        curPylone = null;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        oxygenController = player.GetComponent<OxygenModuleController>();
    }

    private void Update()
    {
        if (curPylone!=null) //if the current Pylone (the one the player is connected to) exists, we test if the player is still tethered to it AND if the pylone is still connected to the network
        {
            if (curPylone.GetComponent<OxygenPyloneController>().TestPlayerConnection() && curPylone.GetComponent<OxygenPyloneController>().connectedToNetwork) //if the player is on a certain pylone, we first test if he's still on this one
            {
                Debug.Log("PLAYER STILL IN NETWORK");
            }
            else //the player has left the curPylone's range, so we will begin our search at the next frame
            {
                Debug.Log("PLAYER LEFT CUR PYLONE");
                curPylone.GetComponent<OxygenPyloneController>().isActivePylone = false;
                curPylone =null;
            }
        }
        else //the player has left all pylones' reach, so we test to see if he's in range of one
        {
            int rateDecrease=0;
            foreach (var pyl in pylonesNetwork)
            {
                rateDecrease++;
                OxygenPyloneController controller = pyl.GetComponent<OxygenPyloneController>();
                if (controller.TestPlayerConnection() && controller.connectedToNetwork) //if for the considered pylone the player is connected (in range) AND the pylone is connected to the network, we connect the player and setup curpylone
                {
                    Debug.Log("PLAYER ENTERS NETWORK");
                    curPylone = pyl;
                    controller.isActivePylone = true;
                    if(pumpOxygenRate - rateDecrease <= 0)
                    {
                        curOxygenRate = 0;
                    }
                    else
                    {
                        curOxygenRate = pumpOxygenRate - rateDecrease;
                    }
                    StartOxygenFlow();
                    return;
                }
            }
            Debug.Log("NO LONGER CONNECTED TO OXYGEN NETWORK");
            StopOxygenFlow();
            curPylone = null;
        }

    }

    /**
     *  Function stopping the oxygen flow
     */
    private void StopOxygenFlow()
    {
        oxygenController.isRecharging = false;
    }

    /**
     *  Function starting the oxygen flow towards the player
     */
    private void StartOxygenFlow()
    {
        oxygenController.isRecharging = true;
        StartCoroutine(oxygenController.AddOxygen(curOxygenRate));
    }
}
