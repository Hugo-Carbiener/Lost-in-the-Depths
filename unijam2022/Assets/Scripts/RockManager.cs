using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockManager : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;

    void HitByRay()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            gameObject.SetActive(false);
            lifetime = 0;
        }
    }
}
