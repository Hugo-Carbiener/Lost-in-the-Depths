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
        RemoveRock(5, 0);
        RemoveRock(6, 0);
        RemoveRock(7, 0);
        RemoveRock(8, 0);
        RemoveRock(9, 0);
        GenerateGrass();
        
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

    private void GenerateGrass()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            int y = 0;
            while (true)
            {
                // dirt tile on the highest level
                if (y == 0 && tilemapArray[x,y] != 0)
                {
                    tilemapArray[x, y] = -1;

                    PaintRock(x, y);
                    break;
                }

                // dirt tile with nothing above it
                if (y > 0 && tilemapArray[x, y] == 1 && tilemapArray[x, y - 1] == 0)
                {
                    print("found");
                    tilemapArray[x, y] = -1;
                    PaintRock(x, y);
                    break;
                }

                // reached to far deep to have dirt
                if (tilemapArray[x, y] > 0)
                {
                    break;
                }

                y++;
            }
        }
    }

    private void PaintTilemap()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                PaintRock(x, y);
            }
        }
    }

    /**
     * destroy the current rock if there is one, instantiate the correct one
     */
    private void PaintRock(int x, int y)
    {
        int value = tilemapArray[x, y];

        GameObject currentlyPlacedRock = GetPlacedRock(x, y);
        if (currentlyPlacedRock != null)
        {
            Destroy(currentlyPlacedRock);
        }

        if (value != 0)
        {
            placedRockList[x, y] = Instantiate(rockDictionary[tilemapArray[x, y]], grid.CellToWorld(new Vector3Int(x, -y, 0)), Quaternion.identity);
        }
    }

    /**
     * Modify array value and calls for an update on the block
     */
    private void RemoveRock(int x, int y)
    {
        tilemapArray[x, y] = 0;
        // update rock
        PaintRock(x, y);
    }

    // block prefab getter
    private GameObject? GetPlacedRock(int x, int y)
    {
        return placedRockList[x, y];
    }
}
