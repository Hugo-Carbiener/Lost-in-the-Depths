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
    /**
     * The tilemap is described by a 2 dimensionnal array
     * Each cell has a int that describes which tile occupies it
     * -1 is for grass tiles
     *  0 is for emptiness
     *  1,2,3,4 are for base tiles
     */
    private int[,] tilemapArray;

    [Header("Tiles prefabs")]
    [SerializeField] private List<GameObject> baseTiles;
    [SerializeField] private List<GameObject> oreTiles;
    [SerializeField] private GameObject grassTile;
    GameObject[,] placedRockList;
    Dictionary<int, GameObject> rockDictionary;


    private void Awake()
    {
        placedRockList = new GameObject[mapWidth, mapHeight];
        rockDictionary = new Dictionary<int, GameObject>();
        for (int rockIndex = 0; rockIndex < baseTiles.Count; rockIndex++)
        {
            rockDictionary.Add(rockIndex + 1, baseTiles[rockIndex]);
        }
        // add an occurence of the grass tile
        rockDictionary.Add(-1, grassTile);

        // initialize array
        tilemapArray = new int[mapWidth, mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                tilemapArray[x, y] = 0;
            }
        }
    }

    private void Start()
    {
        GenerateBaseTilemap();
        PaintTilemap();
        print(GetPlacedRock(5, 10));
        Destroy(GetPlacedRock(5, 10));
        Destroy(GetPlacedRock(7, 10));
        Destroy(GetPlacedRock(9, 10));
        Destroy(GetPlacedRock(5, 15));
        Destroy(GetPlacedRock(5, 20));
        Destroy(GetPlacedRock(5, 25));
    }

    /**
     * Fills the tilemap with base blocks according to the depth
     */
    private void GenerateBaseTilemap()
    {
        int layerAmount = baseTiles.Count - 1;
        int layerHeight = mapHeight / layerAmount;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                tilemapArray[x, y] = (y / layerHeight) + 1;
            }
        }
    }

    /*private void GenerateGrass()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            int y = 0;
            while (true)
            {
                if (y == 0 && GetPlacedRock(x, y) != null || )
                break
            }
        }
    }*/

    private void PaintTilemap()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int value = tilemapArray[x, y];
                
                if (value != 0)
                {
                    placedRockList[x, y] = Instantiate(rockDictionary[tilemapArray[x, y]], grid.CellToWorld(new Vector3Int(x, -y, 0)), Quaternion.identity);
                }
            }
        }
    }

    private GameObject? GetPlacedRock(int x, int y)
    {
        return placedRockList[x, y];
    }
}
