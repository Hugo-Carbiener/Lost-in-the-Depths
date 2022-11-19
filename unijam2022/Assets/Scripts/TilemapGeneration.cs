using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class TilemapGeneration : MonoBehaviour
{
    public static TilemapGeneration Instance { get; private set; }

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
     *  11,12,13,14 are for ores
     */
    private int[,] tilemapArray;

    [Header("Ore variables")]
    [SerializeField] private int averageVeinSize;
    [SerializeField, Range(0, 1)] private float oreDensity;
    [SerializeField] int OffsetFromSurface;
    [SerializeField] int oreApparitionRangeOutOfLayer;

    [Header("Tiles prefabs")]
    [SerializeField] private List<GameObject> baseTiles;
    [SerializeField] private List<GameObject> oreTiles;
    [SerializeField] private GameObject grassTile;
    GameObject[,] placedRockArray;
    Dictionary<int, GameObject> rockDictionary;

    [Header("FogOfWar prefabs")]
    [SerializeField] private GameObject FogOfWarTile;
    GameObject[,] fogOfWarArray;

    private void Awake()
    {
        // singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }


        placedRockArray = new GameObject[mapWidth, mapHeight];
        fogOfWarArray = new GameObject[mapWidth, mapHeight];
        rockDictionary = new Dictionary<int, GameObject>();
       
        // add rocks in dictionnary
        for (int rockIndex = 0; rockIndex < baseTiles.Count; rockIndex++)
        {
            rockDictionary.Add(rockIndex + 1, baseTiles[rockIndex]);
            baseTiles[rockIndex].SetActive(false);
        }
        // add ores in dictionnary
        for (int oreIndex = 0; oreIndex< oreTiles.Count; oreIndex++ )
        {
            rockDictionary.Add(oreIndex + 11, oreTiles[oreIndex]);
            oreTiles[oreIndex].SetActive(false);
        }
        // add grass in dictionnary
        rockDictionary.Add(-1, grassTile);
        grassTile.SetActive(false);

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
        GenerateOres();

        GenerateFogOfWar();
    }

    /**
     * Fills the tilemap with base blocks according to the depth
     */
    private void GenerateBaseTilemap()
    {
        //map info
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

    /**
     * Generate and paint grass
     */
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

    private void GenerateOres()
    {
        int oreAmount = (int) (oreDensity * mapHeight * mapWidth);
        
        //map info
        int layerAmount = baseTiles.Count - 1;
        int layerHeight = mapHeight / layerAmount;

        for (int oreIndex= 0; oreIndex < oreAmount;)
        {
            // for each ore, calculate y position with a gaussian 
            int oreVersion = Random.Range(0, oreTiles.Count);
            int yPos = (int) Utils.RandomGaussian(layerHeight * oreVersion - oreApparitionRangeOutOfLayer, layerHeight * (oreVersion + 1) + oreApparitionRangeOutOfLayer);
            int xPos = Random.Range(0, mapWidth);
            
            // ensure coordinates are within bounds
            Vector2Int correctedCoordinates = CheckMapBounds(xPos, yPos);

            if (yPos >= OffsetFromSurface)
            {
                int veinSize = averageVeinSize + Random.Range(-averageVeinSize / 2, averageVeinSize / 2);
                oreIndex += veinSize;
                GenerateOreVein(correctedCoordinates.x, correctedCoordinates.y, veinSize, oreVersion);
            }
        }
    }

    private void GenerateOreVein(int x, int y, int oreAmount, int oreVersion)
    {
        List<Vector2Int> oresPlaced = new List<Vector2Int>();

        // place first ore bloc
        tilemapArray[x, y] = oreVersion + 11;
        oresPlaced.Add(new Vector2Int(x, y));

        // place each block of ore in the vein
        while (oresPlaced.Count < oreAmount)
        {
            int orePosRd = Random.Range(0, oresPlaced.Count);
            int directionRd = Random.Range(0, 4);

            Vector2Int targetPos = oresPlaced[orePosRd] + Utils.directions[directionRd];
            if (!(targetPos.x >= mapWidth || targetPos.y >= mapHeight || targetPos.x < 0 || targetPos.y < 0))
            {
                tilemapArray[targetPos.x, targetPos.y] = oreVersion + 11;
                oresPlaced.Add(new Vector2Int(targetPos.x, targetPos.y));
            }
        }
        
        // refresh tilemap
        foreach(Vector2Int orePlaced in oresPlaced)
        {
            PaintRock(orePlaced.x, orePlaced.y);
        }
    }

    /**
     * call paint rock on the whole tilemap
     */
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

    private void GenerateFogOfWar()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                UpdateFogOfWar(x, y);
            }
        }
    }

    private void UpdateFogOfWar(int x, int y)
    {
        Vector2Int coordinates = CheckMapBounds(x, y);
        x = coordinates.x;
        y = coordinates.y;
        
        bool shouldBeCovered = true;

        // check if the tile has an empty tile nearby 
        foreach (Vector2Int direction in Utils.directions) { 
            Vector2Int targetPos = new Vector2Int(direction.x + x, direction.y + y);
            try
            {
                if (tilemapArray[targetPos.x, targetPos.y] == 0)
                {
                    shouldBeCovered = false;
                    break;
                }
            }
            catch (System.IndexOutOfRangeException ex) { }
        }

        // if there is no fog where there should be, add one
        if (shouldBeCovered && fogOfWarArray[x,y] == null)
        {
            fogOfWarArray[x, y] = Instantiate(FogOfWarTile, grid.CellToWorld(new Vector3Int(x, -y, 0)), Quaternion.identity);
        }

        // if there is fog were there shouldn't remove it
        if (!shouldBeCovered && fogOfWarArray[x, y] != null)
        {
            Destroy(fogOfWarArray[x, y]);
            fogOfWarArray[x, y] = null;
        }
    }

    private void UpdateForOfWarAround(int x, int y)
    {
        print("Update fow");
        foreach (Vector2Int direction in Utils.directions) {
            Vector2Int targetPos = new Vector2Int(direction.x + x, direction.y + y);
            UpdateFogOfWar(targetPos.x, targetPos.y);
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

        // we instantiate a block, store its coordinates in RockManager for later use and set it active
        if (value != 0)
        {
            GameObject rockToPlace = Instantiate(rockDictionary[tilemapArray[x, y]], grid.CellToWorld(new Vector3Int(x, -y, 0)), Quaternion.identity);
            rockToPlace.GetComponent<RockManager>().SetCoordinates(new Vector2Int(x, y));
            rockToPlace.SetActive(true);
            placedRockArray[x, y] = rockToPlace;
        }
    }

    /**
     * Modify array value and calls for an update on the block
     */
    public void RemoveRock(int x, int y)
    {
        tilemapArray[x, y] = 0;
        PaintRock(x, y);
        UpdateForOfWarAround(x, y);
    }

    // block prefab getter
    private GameObject? GetPlacedRock(int x, int y)
    {
        return placedRockArray[x, y];
    }

    private Vector2Int CheckMapBounds(int x, int y)
    {
        int xRes = x;
        int yRes = y;

        if (x < 0)
        {
            xRes = 0;
        }
        if (x > mapWidth - 1)
        {
            xRes = mapWidth - 1;
        }
        if (y < 0)
        {
            yRes = 0;
        }
        if (y > mapHeight - 1)
        {
            yRes = mapHeight - 1;
        }

        return new Vector2Int(xRes, yRes);
    }
}
