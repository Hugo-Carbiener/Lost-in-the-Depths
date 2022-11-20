using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockManager : MonoBehaviour
{
    [Header("Rock variables")]
    [SerializeField] private AudioSource breakingSound;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private ResourcesType resourceType;
    private Vector2Int coordinates;

    [SerializeField] private GameObject particles;

    private void Awake()
    {
        if(particles != null)
        {
            particles.SetActive(false);
        }
        breakingSound = gameObject.GetComponent<AudioSource>();
    }

    void HitByRay()
    {
        if (particles != null)
        {
            particles.SetActive(true);
        }
        if (resourceType != ResourcesType.Unbreakable)
        {
            lifetime -= Time.deltaTime;
            if (lifetime > 0 && lifetime < 0.4f)
            {
                if (!breakingSound.isPlaying) breakingSound.Play();
            }
            if (lifetime <= 0)
            {
                PlayerManager._instance.AddToResourcesInventory(resourceType);
                lifetime = 0;
                TilemapGeneration.Instance.RemoveRock(coordinates.x, coordinates.y);
            }
        }
    }

    void NoLongerHit()
    {
        if (particles != null)
        {
            particles.SetActive(false);
        }
    }

    public void SetCoordinates(Vector2Int coords)
    {
        coordinates = coords;
    }
}
