using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class TilemapGeneration : MonoBehaviour
{
    public static TilemapGeneration Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private Grid grid;
    [SerializeField] private Transform blocContainer;
    [SerializeField] private Transform backgroundBlocContainer;
    [SerializeField] private Transform foWContainer;

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
    int layerAmount;
    int layerHeight;

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

    [Header("Background variables")]
    [SerializeField] private int depthOffset;

    [Header("FogOfWar prefabs")]
    [SerializeField] private GameObject FogOfWarTile;
    GameObject[,] fogOfWarArray;
    List<Vector2Int> range;

    [Header("Elevator variables")]
    [SerializeField] private int shaftDepth;
    [SerializeField] private GameObject elevator;

    [Header("Landscaping")]
    [SerializeField] private int maxEntranceDepth;

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
        for (int oreIndex = 0; oreIndex < oreTiles.Count; oreIndex++)
        {
            rockDictionary.Add(oreIndex + 11, oreTiles[oreIndex]);
            oreTiles[oreIndex].SetActive(false);
        }
        // add grass in dictionnary
        rockDictionary.Add(-1, grassTile);
        grassTile.SetActive(false);
        elevator.SetActive(false);

        // initialize array
        tilemapArray = new int[mapWidth, mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                tilemapArray[x, y] = 0;
            }
        }

        layerAmount = baseTiles.Count;
        layerHeight = mapHeight / layerAmount;
    }

    private void Start()
    {
        GenerateBaseTilemap();
        PaintTilemap();

        GenerateBackground();
        GenerateOres();

        GenerateEntrance();
        GenerateGrass();

        GenerateElevator();

        GenerateFogOfWar();
    }

    /**
     * Fills the tilemap with base blocks according to the depth
     */
    private void GenerateBaseTilemap()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                tilemapArray[x, y] = (y / layerHeight) + 1;
            }
        }
    }

    /**
     * Draw a copy of the basic tilemap behind as background
     */
    private void GenerateBackground()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                print("add a bg tile");
                GameObject backgroundToPlace = Instantiate(rockDictionary[tilemapArray[x, y]], grid.CellToWorld(new Vector3Int(x, -y, 0)), Quaternion.identity, backgroundBlocContainer);
                backgroundToPlace.SetActive(true);
            }
        }
        backgroundBlocContainer.transform.position += Vector3.forward * depthOffset;
    }

    private void GenerateEntrance()
    {
        for(int i = 0; i < mapWidth / 2; i++)
        {
            int depth = (i * maxEntranceDepth) / (mapWidth / 2);   
            for(int y = 0; y < depth; y++)
            {
                RemoveRock(i, y);
                RemoveRock(mapWidth - i, y);
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
                if (y == 0 && tilemapArray[x, y] != 0)
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
        int oreAmount = (int)(oreDensity * mapHeight * mapWidth);

        for (int oreIndex = 0; oreIndex < oreAmount;)
        {
            // for each ore, calculate y position with a gaussian 
            int oreVersion = Random.Range(0, oreTiles.Count);
            int yPos = (int)Utils.RandomGaussian(layerHeight * oreVersion - oreApparitionRangeOutOfLayer, layerHeight * (oreVersion + 1) + oreApparitionRangeOutOfLayer);
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

            Vector2Int targetPos = oresPlaced[orePosRd] + Utils.directNeihbors[directionRd];
            if (!(targetPos.x >= mapWidth || targetPos.y >= mapHeight || targetPos.x < 0 || targetPos.y < 0))
            {
                tilemapArray[targetPos.x, targetPos.y] = oreVersion + 11;
                oresPlaced.Add(new Vector2Int(targetPos.x, targetPos.y));
            }
        }

        // refresh tilemap
        foreach (Vector2Int orePlaced in oresPlaced)
        {
            PaintRock(orePlaced.x, orePlaced.y);
        }
    }

    private void GenerateElevator()
    {
        // generate shaft
        int[] xs = new int[2] { mapWidth / 2, (mapWidth / 2) + 1 };
        foreach (int x in xs)
        {
            for (int y = 0; y < shaftDepth; y++)
            {
                RemoveRock(x, y);
            }
        }

        Vector3 elevatorPos = (grid.CellToWorld(new Vector3Int(xs[0], -(maxEntranceDepth - 1), 0)) + grid.CellToWorld(new Vector3Int(xs[0], -(maxEntranceDepth - 1), 0))) / 2 + Vector3.right * 0.5f;
        GameObject elev = Instantiate(elevator, elevatorPos, Quaternion.identity);
        elev.GetComponent<ElevatorController>().SetDepth(shaftDepth - maxEntranceDepth);
        elev.SetActive(true);
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

    /**
     * fills the map with fow
     */
    private void GenerateFogOfWar()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                UpdateFogOfWarAt(x, y);
            }
        }
    }

    /**
     * Update the fog of war by checking the tile around position with different tiles
     */
    private void UpdateFogOfWarAt(int x, int y, bool isInInit = true)
    {
        Vector2Int coordinates = CheckMapBounds(x, y);
        x = coordinates.x;
        y = coordinates.y;

        bool shouldBeCovered = true;

        if (isInInit)
        {
            range = Utils.directNeihbors;
        }

        // check if the tile has an empty tile nearby 
        foreach (Vector2Int direction in range)
        {
            Vector2Int targetPos = new Vector2Int(direction.x + x, direction.y + y);
            try
            {
                if (tilemapArray[targetPos.x, targetPos.y] == 0 || tilemapArray[targetPos.x, targetPos.y] == -1)
                {
                    shouldBeCovered = false;
                    break;
                }
            }
            catch (System.IndexOutOfRangeException ex) { }
        }

        // if there is no fog where there should be, add one
        if (shouldBeCovered && fogOfWarArray[x, y] == null)
        {
            fogOfWarArray[x, y] = Instantiate(FogOfWarTile, grid.CellToWorld(new Vector3Int(x, -y, 0)), Quaternion.identity, foWContainer);
        }

        // if there is fog were there shouldn't remove it
        if (!shouldBeCovered && fogOfWarArray[x, y] != null)
        {
            Destroy(fogOfWarArray[x, y]);
            fogOfWarArray[x, y] = null;
        }
    }

    // Call to update the fog of war on the 4 tiles around position
    private void UpdateFogOfWarAround(int x, int y)
    {
        if (y < layerHeight)
        {
            range = Utils.squaredRadius2Sphere;
        }
        else if (y >= layerHeight && y < 2 * layerHeight)
        {
            range = Utils.radius2Sphere;
        }
        else if (y > 2 * layerHeight && y < 3 * layerHeight)
        {
            range = Utils.neihbors;
        }
        else
        {
            range = Utils.directNeihbors;
        }

        foreach (Vector2Int direction in range)
        {
            Vector2Int targetPos = new Vector2Int(direction.x + x, direction.y + y);
            UpdateFogOfWarAt(targetPos.x, targetPos.y, false);
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
            GameObject rockToPlace = Instantiate(rockDictionary[tilemapArray[x, y]], grid.CellToWorld(new Vector3Int(x, -y, 0)), Quaternion.identity, blocContainer);
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
        UpdateFogOfWarAround(x, y);
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
