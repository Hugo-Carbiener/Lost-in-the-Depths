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

    [SerializeField] private TextMeshProUGUI coalNumber;
    [SerializeField] private TextMeshProUGUI goldNumber;
    [SerializeField] private TextMeshProUGUI diamondNumber;

    private void Start()
    {
        oxygenNetwork = GameObject.FindGameObjectWithTag("OxygenNetwork").GetComponent<OxygenNetwork>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        oxygen = player.GetComponent<OxygenModuleController>();
    }

    private void LateUpdate()
    {
        oxygenBar.value = oxygen.curOxygen/oxygen.maxOxygen;
        oxygenRate.text = oxygenNetwork.curOxygenRate.ToString() + " o/s";
        if (PlayerManager._instance.resourcesInventory.ContainsKey(ResourcesType.Coal))
        {
            coalNumber.text = PlayerManager._instance.resourcesInventory[ResourcesType.Coal].ToString();
        }
        if (PlayerManager._instance.resourcesInventory.ContainsKey(ResourcesType.Gold))
        {
            goldNumber.text = PlayerManager._instance.resourcesInventory[ResourcesType.Gold].ToString();
        }
        if (PlayerManager._instance.resourcesInventory.ContainsKey(ResourcesType.Diamond))
        {
            diamondNumber.text = PlayerManager._instance.resourcesInventory[ResourcesType.Diamond].ToString();
        }
    }
}
