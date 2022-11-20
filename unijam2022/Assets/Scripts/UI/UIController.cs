using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private TextMeshProUGUI pylonNumber;

    [SerializeField] private GameObject EscapeMenuUI;

    private void Start()
    {
        oxygenNetwork = GameObject.FindGameObjectWithTag("OxygenNetwork").GetComponent<OxygenNetwork>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        oxygen = player.GetComponent<OxygenModuleController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            EscapeMenuUI.SetActive(true);
        }
    }

    private void LateUpdate()
    {
        oxygenBar.value = oxygen.curOxygen/oxygen.maxOxygen; //updates the oxygenBar
        oxygenRate.text = oxygenNetwork.curOxygenRate.ToString() + " o/s"; //uodates the oxygenRate text
        oxygenBar.value = oxygen.curOxygen/oxygen.maxOxygen;
        oxygenRate.text = oxygenNetwork.curOxygenRate.ToString() + " o/s";
        coalNumber.text = PlayerManager._instance.resourcesInventory[ResourcesType.Coal].ToString();
        goldNumber.text = PlayerManager._instance.resourcesInventory[ResourcesType.Gold].ToString();
        diamondNumber.text = PlayerManager._instance.resourcesInventory[ResourcesType.Diamond].ToString();
        pylonNumber.text = PlayerManager._instance.resourcesInventory[ResourcesType.Pylons].ToString();
    }

    public void OnResume()
    {
        Time.timeScale = 1;
        EscapeMenuUI.SetActive(false);
    }

    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
