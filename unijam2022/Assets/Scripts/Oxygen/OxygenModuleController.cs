using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Component of the Player GameObject that will handle its oxygen consumption
 */
public class OxygenModuleController : MonoBehaviour
{
    public float maxOxygen;
    [HideInInspector] public float curOxygen;
    public float consumptionRate;
    public bool isRecharging;//boolean put at true if player connected to oxygen system (hence recharging)

    private void Start()
    {
        curOxygen = maxOxygen;
        isRecharging = false;
        StartCoroutine(ConsumeOxygen());
    }

    /**
     *  Loop that consumes oxygen
     */
    private IEnumerator ConsumeOxygen()
    {
        yield return new WaitForSeconds(1);
        curOxygen-=consumptionRate;
        //Debug.Log("Current oxygen level : " + curOxygen);
        if (curOxygen <= 0)
        {
            curOxygen = 0;
            //Debug.Log("ALERT : NO OXYGEN REMAINING");
        }
        StartCoroutine(ConsumeOxygen());
    }

    /**
     *  Loop that adds oxygen if player connected to oxygen system
     *  To trigger recharge : start coroutine AND put boolean isRecharging=true if connected
     */
    public IEnumerator AddOxygen(float rate)
    {
        yield return new WaitForSeconds(1);
        curOxygen += rate; //the recharge goes faster than the downsize as to increase by 1 increment per tick of the coroutines
        if (curOxygen >= maxOxygen)
        {
            curOxygen = maxOxygen;
        }
        if (isRecharging) //if the player is still connected to oxygen system, then its oxygen levels continue recharging
        {
            StartCoroutine(AddOxygen(rate));
        }
    }
}
