using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTextureManager : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private RockManager rockManager;

    [Header("Rock texture variables")]
    [SerializeField] private List<GameObject> rockTiles;
    [SerializeField] private List<float> rockLifetime;

    private void Awake()
    {
        if (!rockManager) rockManager = GetComponent<RockManager>();
        SetTexture(0);
    }

    public void SetTexture(int index)
    {
        if (index < 0 || index >= rockTiles.Count)
        {
            return;
        }

        for (int i = 0; i < rockTiles.Count; i++)
        {
            if (i == index)
            {
                rockTiles[i].SetActive(true);
            } else
            {
                rockTiles[i].SetActive(false);
            }
        }
        
        rockManager.lifetime = rockLifetime[index];
    }
}
