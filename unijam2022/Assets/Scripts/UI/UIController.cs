using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 *      Component used by the ui
 * 
 */
public class UIController : MonoBehaviour
{
    private GameObject player;
    private OxygenModuleController oxygen;
    private OxygenNetwork oxygenNetwork;
    [SerializeField] private Slider oxygenBar;
    [SerializeField] private TextMeshProUGUI oxygenRate;

    private void Start()
    {
        oxygenNetwork = GameObject.FindGameObjectWithTag("OxygenNetwork").GetComponent<OxygenNetwork>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        oxygen = player.GetComponent<OxygenModuleController>();
    }

    private void LateUpdate()
    {
        oxygenBar.value = oxygen.curOxygen/oxygen.maxOxygen; //updates the oxygenBar
        oxygenRate.text = oxygenNetwork.curOxygenRate.ToString() + " o/s"; //uodates the oxygenRate text
    }
}
