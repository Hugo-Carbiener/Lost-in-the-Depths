using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Slider oxygenBar;

    private void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        oxygen = player.GetComponent<OxygenModuleController>();
    }

    private void LateUpdate()
    {
        oxygenBar.value = oxygen.curOxygen/oxygen.maxOxygen;
    }
}
