using UnityEngine;
[System.Serializable]

public class Wave
{
    public string waveName;
    public int enemyCount;
}
// Starting on wave system


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float enemySpawnRate;
    private float spawnTimer;

    [SerializeField] Wave[] waves;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= enemySpawnRate)
        {
            spawnTimer = 0.0f;
            SpawnEnemy();
        }
        

    }


    private void SpawnEnemy()
    {
        
        
        
    }
}
