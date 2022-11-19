using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockManager : MonoBehaviour
{
    [Header("Rock variables")]
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private ResourcesType resourceType;
    private Vector2Int coordinates;

    void HitByRay()
    {
        if (resourceType != ResourcesType.Unbreakable)
        {
            lifetime -= Time.deltaTime;
            
            if (lifetime <= 0)
            {
                TilemapGeneration.Instance.RemoveRock(coordinates.x, coordinates.y);
                PlayerManager._instance.AddToResourcesInventory(resourceType);
                lifetime = 0;
            }
        }
    }

    public void SetCoordinates(Vector2Int coords)
    {
        coordinates = coords;
    }
}
