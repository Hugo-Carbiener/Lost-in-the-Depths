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

    [SerializeField] private GameObject EscapeMenuUI;

    [SerializeField] private AudioSource audio;

    [SerializeField] private Canvas deathAnimation;

    private void Start()
    {
        oxygenNetwork = GameObject.FindGameObjectWithTag("OxygenNetwork").GetComponent<OxygenNetwork>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        oxygen = player.GetComponent<OxygenModuleController>();
        if (audio == null)
        {
            audio = gameObject.GetComponentInChildren<AudioSource>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            audio.Play();
            EscapeMenuUI.SetActive(true);
        }
    }

    private void LateUpdate()
    {
        oxygenBar.value = oxygen.curOxygen/oxygen.maxOxygen; //updates the oxygenBar
        oxygenRate.text = oxygenNetwork.curOxygenRate.ToString() + " o/s"; //uodates the oxygenRate text
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

    public void OnResume()
    {
        Time.timeScale = 1;
        audio.Play();
        EscapeMenuUI.SetActive(false);
    }

    public void OnBackToMainMenu()
    {
        audio.Play();
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void TriggerDeath()
    {
        deathAnimation.gameObject.SetActive(true);
        deathAnimation.GetComponent<Animator>().Play("Death");
    }
}
