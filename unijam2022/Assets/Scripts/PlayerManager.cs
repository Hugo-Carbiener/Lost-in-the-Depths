using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager _instance;

    public Dictionary<ResourcesType, int> resourcesInventory;
    private int laserLevel = 1;
    public float[] coefficientsLaser;
    [SerializeField] private int maxLaserLevel = 3;

    private void Awake()
    {
        Debug.Assert(PlayerManager._instance == null);
        PlayerManager._instance = this;
        resourcesInventory = new();
        resourcesInventory.Add(ResourcesType.Pylons, 3);
        resourcesInventory.Add(ResourcesType.Coal, 0);
        resourcesInventory.Add(ResourcesType.Gold, 0);
        resourcesInventory.Add(ResourcesType.Diamond, 0);
    }

    public void AddToResourcesInventory(ResourcesType resource)
    {
        if (resourcesInventory.ContainsKey(resource))
        {
            resourcesInventory[resource]++;
        }
        else
        {
            resourcesInventory.Add(resource, 1);
        }
    }

    public int GetLaserLevel()
    {
        return laserLevel;
    }

    public int GetMaxLaserLevel()
    {
        return maxLaserLevel;
    }

    public void IncrementLaserLevel()
    {
        laserLevel++;
    }
}
