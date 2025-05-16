using UnityEngine;
using UnityEngine.Tilemaps;

public class PropSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase spawnMarkerTile;

    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    public GameObject[] propPrefabs;
    [Range(0f, 1f)]
    public float spawnChance = 0.5f;
    public GameObject[] houseprops;
    public int housesToSpawn = 10;

    void Start()
    {
        SpawnPropsOnMatchingTiles();
        SpawnHouses();
    }

    void SpawnPropsOnMatchingTiles()
    {
        if (tilemap == null || spawnMarkerTile == null || propPrefabs == null || propPrefabs.Length == 0)
        {
            Debug.LogWarning("Missing references or empty prop array.");
            return;
        }

        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase currentTile = tilemap.GetTile(pos);
            if (currentTile == spawnMarkerTile && Random.value <= spawnChance)
            {
                Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
                GameObject selectedProp = propPrefabs[Random.Range(0, propPrefabs.Length)];
                Instantiate(selectedProp, worldPos, Quaternion.identity);
            }
        }
    }

    void SpawnHouses()
    {
        if (houseprops.Length == 0) return;

        for (int i = 0; i < housesToSpawn; i++)
        {
            GameObject propToSpawn = houseprops[Random.Range(0, houseprops.Length)];

            Vector2 spawnPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            Instantiate(propToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}