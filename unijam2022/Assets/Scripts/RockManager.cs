using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockManager : MonoBehaviour
{
    [Header("Rock variables")]
    [SerializeField] private AudioSource breakingSound;
    public float lifetime { private get; set; } = 1;
    [SerializeField] private ResourcesType resourceType;
    private Vector2Int coordinates;
    private float startLifeTime;

    [Header("Particles")]
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject particlesNearDeath;
    private bool nearDeath;

    private void Awake()
    {
        if(particles != null)
        {
            particles.SetActive(false);
        }
        if (particlesNearDeath != null)
        {
            particlesNearDeath.SetActive(false);
        }
        breakingSound = gameObject.GetComponent<AudioSource>();
        nearDeath = false;
        startLifeTime = lifetime;
    }

    void HitByRay()
    {
        if (nearDeath)
        {
            if (particlesNearDeath != null)
            {
                particlesNearDeath.SetActive(true);
                particles.SetActive(false);
            }
        }
        else
        {
            if (particles != null)
            {
                particles.SetActive(true);
            }
        }
        if (lifetime <= startLifeTime / 3f)
        {
            nearDeath = true;
        }
        if (resourceType != ResourcesType.Unbreakable)
        {
            lifetime -= Time.deltaTime * PlayerManager._instance.coefficientsLaser[PlayerManager._instance.GetLaserLevel() - 1];
            if (lifetime > 0 && lifetime < 0.2f)
            {
                if (!breakingSound.isPlaying) breakingSound.Play();
            }
            if (lifetime <= 0)
            {
                if (resourceType == ResourcesType.FinalOre)
                {
                    GetComponent<FinaRockController>().StartVictory();
                }
                PlayerManager._instance.AddToResourcesInventory(resourceType);
                lifetime = 0;
                TilemapGeneration.Instance.RemoveRock(coordinates.x, coordinates.y);
            }
        }
    }

    void NoLongerHit()
    {
        if (particlesNearDeath != null)
        {
            particlesNearDeath.SetActive(false);
        }
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
