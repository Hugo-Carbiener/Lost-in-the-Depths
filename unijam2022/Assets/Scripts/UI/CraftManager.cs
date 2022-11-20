using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftManager : MonoBehaviour
{
    [SerializeField] private int coalRequiredForPylons = 3;
    [SerializeField] private int goldRequiredForPylons = 0;
    [SerializeField] private int diamondRequiredForPylons = 0;
    [SerializeField] private int pylonsCrafted = 3;

    [SerializeField] private TextMeshProUGUI nbCoalRequiredForPylons;
    [SerializeField] private TextMeshProUGUI nbGoldRequiredForPylons;
    [SerializeField] private TextMeshProUGUI nbDiamondRequiredForPylons;
    [SerializeField] private Button craftPylonsButton;

    [SerializeField] private int coalRequiredForLaser = 3;
    [SerializeField] private int goldRequiredForLaser = 0;
    [SerializeField] private int diamondRequiredForLaser = 0;

    [SerializeField] private TextMeshProUGUI nbCoalRequiredForLaser;
    [SerializeField] private TextMeshProUGUI nbGoldRequiredForLaser;
    [SerializeField] private TextMeshProUGUI nbDiamondRequiredForLaser;
    [SerializeField] private Button upgradeLaserButton;

    private void Awake()
    {
        nbCoalRequiredForPylons.text = coalRequiredForPylons.ToString();
        nbGoldRequiredForPylons.text = goldRequiredForPylons.ToString();
        nbDiamondRequiredForPylons.text = diamondRequiredForPylons.ToString();
        nbCoalRequiredForLaser.text = coalRequiredForLaser.ToString();
        nbGoldRequiredForLaser.text = goldRequiredForLaser.ToString();
        nbDiamondRequiredForLaser.text = diamondRequiredForLaser.ToString();
    }

    private void Update()
    {
            if (PlayerManager._instance.resourcesInventory[ResourcesType.Coal] >= coalRequiredForPylons && PlayerManager._instance.resourcesInventory[ResourcesType.Gold] >= goldRequiredForPylons && PlayerManager._instance.resourcesInventory[ResourcesType.Diamond] >= diamondRequiredForPylons && PlayerManager._instance.GetLaserLevel() < PlayerManager._instance.GetMaxLaserLevel())
            {
                craftPylonsButton.interactable = true;
            }
            else
            {
                craftPylonsButton.interactable = false;
            }
            if (PlayerManager._instance.resourcesInventory[ResourcesType.Coal] >= coalRequiredForLaser && PlayerManager._instance.resourcesInventory[ResourcesType.Gold] >= goldRequiredForLaser && PlayerManager._instance.resourcesInventory[ResourcesType.Diamond] >= diamondRequiredForLaser)
            {
                upgradeLaserButton.interactable = true;
            }
            else
            {
                upgradeLaserButton.interactable = false;
            }
    }

    public void CraftPylons()
    {
        PlayerManager._instance.resourcesInventory[ResourcesType.Coal] -= coalRequiredForPylons;
        PlayerManager._instance.resourcesInventory[ResourcesType.Gold] -= goldRequiredForPylons;
        PlayerManager._instance.resourcesInventory[ResourcesType.Diamond] -= diamondRequiredForPylons;
        PlayerManager._instance.resourcesInventory[ResourcesType.Pylons] += pylonsCrafted;
        Debug.Log(PlayerManager._instance.resourcesInventory[ResourcesType.Pylons]);
    }

    public void UpgradeLaser()
    {
        PlayerManager._instance.resourcesInventory[ResourcesType.Coal] -= coalRequiredForLaser;
        PlayerManager._instance.resourcesInventory[ResourcesType.Gold] -= goldRequiredForLaser;
        PlayerManager._instance.resourcesInventory[ResourcesType.Diamond] -= diamondRequiredForLaser;
        PlayerManager._instance.IncrementLaserLevel();
    }
}
