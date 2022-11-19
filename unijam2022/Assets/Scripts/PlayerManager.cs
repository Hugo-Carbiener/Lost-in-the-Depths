using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager _instance;

    public Dictionary<ResourcesType, int> resourcesInventory;

    private void Awake()
    {
        Debug.Assert(PlayerManager._instance == null);
        PlayerManager._instance = this;
        resourcesInventory = new();
        resourcesInventory.Add(ResourcesType.Pylons, 3);
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
}
