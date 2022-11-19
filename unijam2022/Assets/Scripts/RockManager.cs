using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockManager : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private ResourcesType resourceType;

    void HitByRay()
    {
        if (resourceType != ResourcesType.Unbreakable)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                gameObject.SetActive(false);
                PlayerManager._instance.AddToResourcesInventory(resourceType);
                lifetime = 0;
            }
        }
    }
}
