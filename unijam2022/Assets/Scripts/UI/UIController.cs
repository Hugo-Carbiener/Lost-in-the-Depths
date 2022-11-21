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
    [SerializeField] private GameObject CraftMenuUI;

    [SerializeField] private AudioSource uiAudio;

    [SerializeField] private Canvas deathAnimation;
    [SerializeField] private Canvas victoryAnimation;

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
            uiAudio.Play();
            EscapeMenuUI.SetActive(true);
        }
    }

    private void LateUpdate()
    {
        oxygenBar.value = oxygen.curOxygen/oxygen.maxOxygen; //updates the oxygenBar
        oxygenRate.text = oxygenNetwork.curOxygenRate.ToString() + " o/s"; //uodates the oxygenRate text
        coalNumber.text = PlayerManager._instance.resourcesInventory[ResourcesType.Coal].ToString();
        goldNumber.text = PlayerManager._instance.resourcesInventory[ResourcesType.Gold].ToString();
        diamondNumber.text = PlayerManager._instance.resourcesInventory[ResourcesType.Diamond].ToString();
        pylonNumber.text = PlayerManager._instance.resourcesInventory[ResourcesType.Pylons].ToString();
    }

    public void OnResumeEscapeMenu()
    {
        Time.timeScale = 1;
        uiAudio.Play();
        EscapeMenuUI.SetActive(false);
    }

    public void OnResumeCraftMenu()
    {
        Time.timeScale = 1;
        uiAudio.Play();
        CraftMenuUI.SetActive(false);
    }

    public void OnBackToMainMenu()
    {
        uiAudio.Play();
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void TriggerDeath()
    {
        deathAnimation.gameObject.SetActive(true);
        deathAnimation.GetComponent<Animator>().Play("Death");
    }

    public void TriggerVictory()
    {
        victoryAnimation.gameObject.SetActive(true);
        victoryAnimation.GetComponent<Animator>().Play("Victory");
    }
}
