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

    private void Awake()
    {
        nbCoalRequiredForPylons.text = coalRequiredForPylons.ToString();
        nbGoldRequiredForPylons.text = goldRequiredForPylons.ToString();
        nbDiamondRequiredForPylons.text = diamondRequiredForPylons.ToString();
    }

    private void Update()
    {
            if (PlayerManager._instance.resourcesInventory[ResourcesType.Coal] >= coalRequiredForPylons && PlayerManager._instance.resourcesInventory[ResourcesType.Gold] >= goldRequiredForPylons && PlayerManager._instance.resourcesInventory[ResourcesType.Diamond] >= diamondRequiredForPylons)
            {
                craftPylonsButton.interactable = true;
            }
            else
            {
                craftPylonsButton.interactable = false;
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
}
