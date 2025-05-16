using UnityEngine;
using UnityEngine.Tilemaps;

public class CorruptSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase spawnMarkerTile;

    public GameObject[] propPrefabs;
    [Range(0f, 1f)]
    public float spawnChance = 0.5f;

    void Start()
    {
        SpawnPropsOnMatchingTiles();
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
}
