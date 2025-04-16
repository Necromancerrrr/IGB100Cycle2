using UnityEngine;
[System.Serializable]

public class Wave
{
    [Tooltip("Name of the wave... pretty obvious..")]
    [SerializeField] string waveName;

    [Tooltip("How many enemies spawn per spawn cycle")]
    public int enemyDensity;

    [Tooltip("How fast each spawn cycle is (seconds)")]
    public float spawnFrequency;

    [Tooltip("Types of enemies that will be randomly spawned this wave")]
    public GameObject[] enemyType;
}


public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Distance enemies will spawn from the centre of the screen e.g. 1 = 1 screen size")]
    [SerializeField] float spawnDistance = 1f;

    [Tooltip("How fast each wave will be (seconds)")]
    [SerializeField] float waveFrequency;

    [Tooltip("How many enemies can be in the game world at once")]
    [SerializeField] int maxEnemyCapacity;

    [Tooltip("Array of all waves")]
    [SerializeField] Wave[] waves;

    private GameObject[] enemiesSpawned;

    private float spawnTimer;
    private float waveTimer;

    private Wave currentWave;
    private int currentWaveNumber = 0;

    void Update()
    {
        currentWave = waves[currentWaveNumber];

        enemiesSpawned = GameObject.FindGameObjectsWithTag("Enemy");

        waveTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= currentWave.spawnFrequency && enemiesSpawned.Length < maxEnemyCapacity)
        {
            spawnTimer = 0.0f;
            for (int i = 0; i < currentWave.enemyDensity; i++)
            {
                SpawnEnemy();
            }
        }

        if (waveTimer >= waveFrequency)
        {
            currentWaveNumber++;
            waveTimer = 0;
        }
    }

    void SpawnEnemy()
    {
        Vector2 randomPosition;
        GameObject randomEnemy = currentWave.enemyType[Random.Range(0, currentWave.enemyType.Length)];
        
        if (Random.Range(0f, 1f) > 0.5f) // Spawn on the sides of the screen
        {
            randomPosition.y = Random.Range(0.5f - spawnDistance, 0.5f + spawnDistance);
            
            if (Random.Range(0f, 1f) > 0.5f) // Spawn right
            {
                randomPosition.x = 0.5f + spawnDistance;
            }
            else // Spawn left
            {
                randomPosition.x = 0.5f - spawnDistance;
            }
        }
        else // Spawn on top/bottom of the screen
        {
            randomPosition.x = Random.Range(0.5f - spawnDistance, 0.5f + spawnDistance);

            if (Random.Range(0f, 1f) > 0.5f) // Spawn top
            {
                randomPosition.y = 0.5f + spawnDistance;
            }
            else // Spawn bottom
            {
                randomPosition.y = 0.5f - spawnDistance;
            }
        }

        Vector2 spawnPos = Camera.main.ViewportToWorldPoint(randomPosition);
        Instantiate(randomEnemy, spawnPos, Quaternion.identity);
    }

}
