using UnityEngine;
[System.Serializable]

public class PropSpawner : BaseSpawner
{
    [Tooltip("Distance things will spawn")]
    [SerializeField] float spawnDistance = 0.5f;
    [SerializeField] int maxProps;
    [SerializeField] GameObject[] propType;
    

    private GameObject[] propsSpawned; // delete oldest 

    private float spawnTimer;

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= 1 && propsSpawned.Length <= maxProps)
        {
            Vector2 randomPosition =  base.GetRandomPosition(spawnDistance);
            SpawnProp(randomPosition);
            spawnTimer = 0;
        }
    }

    void SpawnProp(Vector2 randomPosition)
    {
        GameObject randomProp = propType[Random.Range(0, propType.Length)];
        
        Vector2 spawnPos = Camera.main.ViewportToWorldPoint(randomPosition);
        Instantiate(randomProp, spawnPos, Quaternion.identity);
    }
}
