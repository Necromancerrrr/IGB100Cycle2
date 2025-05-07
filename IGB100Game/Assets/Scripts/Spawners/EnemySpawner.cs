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

    [Tooltip("How many groups each cycle will spawn, set to 0 if no clustering")]
    public int enemyGroupCount;

    [Tooltip("Types of enemies that will be randomly spawned this wave")]
    public GameObject[] enemyType;

    [Tooltip("Enemy spawn chance %, corresponds to position of enemyType Array (above^), float from 0.0 to 1.0")]
    public float[] enemySpawnChance;

    [Tooltip("Click and drag in a boss if you want ONE to spawn this wave")]
    public GameObject bossSpawn;
}



public class EnemySpawner : BaseSpawner
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

    private float upperBoundSpawnChance;
    private bool bossSpawned;

    void Start()
    {
        currentWave = waves[currentWaveNumber];
        upperBoundSpawnChance = GenerateUpperBoundSpawnChance();
        
    }

    void Update()
    {
        enemiesSpawned = GameObject.FindGameObjectsWithTag("Enemy");

        waveTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= currentWave.spawnFrequency && enemiesSpawned.Length < maxEnemyCapacity) // if spawning can happen
        {
            spawnTimer = 0.0f;
            if (currentWave.enemyGroupCount > 0)
            {
                for (int i = 0; i < currentWave.enemyGroupCount; i++) // if enemies are spawning in groups
                {
                    Vector2 randomPosition = base.GetRandomPosition(spawnDistance);

                    for (int j = 0; j < currentWave.enemyDensity; j++)
                    {
                        SpawnEnemy(randomPosition);
                        randomPosition += new Vector2(Random.Range(-0.0001f, 0.0001f), Random.Range(-0.0001f, 0.0001f)); // No enemy spawn overlap
                    }
                }
            }
            else // no groups
            {
                for (int i = 0; i < currentWave.enemyDensity; i++)
                {
                    Vector2 randomPosition =  base.GetRandomPosition(spawnDistance);
                    SpawnEnemy(randomPosition);
                }
            }
        }

        if (waveTimer >= waveFrequency)
        {
            currentWaveNumber++;
            currentWave = waves[currentWaveNumber];
            waveTimer = 0;
            bossSpawned = false;
            upperBoundSpawnChance = GenerateUpperBoundSpawnChance();
        }
        
        // Spawn in ONE boss if specified
        if (bossSpawned == false && currentWave.bossSpawn != null)
        {
            Vector2 randomPosition =  base.GetRandomPosition(spawnDistance);
            Vector2 spawnPos = Camera.main.ViewportToWorldPoint(randomPosition);
            Instantiate(currentWave.bossSpawn, spawnPos, Quaternion.identity);
            bossSpawned = true;
        }
    }
    

    void SpawnEnemy(Vector2 randomPosition)
    {
        GameObject randomEnemy = RandomEnemyToBeSpawned(upperBoundSpawnChance);
        //currentWave.enemyType[Random.Range(0, currentWave.enemyType.Length)];
        
        Vector2 spawnPos = Camera.main.ViewportToWorldPoint(randomPosition);
        Instantiate(randomEnemy, spawnPos, Quaternion.identity);
    }

    float GenerateUpperBoundSpawnChance()
    {
        float sum = 0;
        foreach (float f in currentWave.enemySpawnChance)
        {
            sum += f;
        }
        return sum;
    }

    GameObject RandomEnemyToBeSpawned(float upperBound)
    {
        float randomFloat = Random.Range(0.0f, upperBound);

        float sumOfChances = 0.0f; // incrementing sum of the spawnchances'

        int numberOfSpawnChances = currentWave.enemySpawnChance.Length;
        int numberOfEnemies = currentWave.enemyType.Length;

        // error checks
        if (numberOfSpawnChances != numberOfEnemies)
        {
            Debug.LogError("enemySpawnChance and enemyType arrays are not the same length");
            return null;
        }
        

        for (int i = 0; i < currentWave.enemySpawnChance.Length; i++)
        {
            sumOfChances += currentWave.enemySpawnChance[i];
            if (randomFloat < sumOfChances)
            {
                return currentWave.enemyType[i];
            }
        }
        return null;
    }
}