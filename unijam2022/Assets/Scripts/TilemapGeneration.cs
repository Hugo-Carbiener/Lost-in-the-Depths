using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class TilemapGeneration : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Grid grid;

    [Header("Map variables")]
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField] private List<GameObject> baseTiles;
    [SerializeField] private List<GameObject> oreTiles;
    [SerializeField] private int[][] tilemap;

    private void Awake()
    {
        int[] line = Enumerable.Repeat(0, mapWidth).ToArray();
        tilemap = Enumerable.Repeat(line, mapHeight).ToArray();
    }

    private void Start()
    {
        GenerateBaseTilemap();
    }

    /**
     * Fills the tilemap with base blocks according to the depth
     */
    private void GenerateBaseTilemap()
    {
        int layerAmount = baseTiles.Count;
        print(layerAmount);
        int layerHeight = mapHeight / layerAmount;
        print(layerHeight);

        for (int x = 0; x < mapHeight; x++)
        {
            for (int y = 0; y < mapWidth; y++)
            {
                int value = x / layerHeight;
                tilemap[x][y] = value;
            }
        }
    }
}
