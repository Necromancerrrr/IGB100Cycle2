using UnityEngine;
[System.Serializable]

public class Wave
{
    public string waveName;
    public int enemyDensity;
    // Types of enemies spawned in wave
    // wave frequency
}
// Starting on wave system


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float spawnFrequency;
    private float spawnTimer;

    [SerializeField] Wave[] waves;
    [SerializeField] GameObject tempEnemy;
    // Max Enemy Spawn

    void Update()
    {
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= spawnFrequency)
        {
            spawnTimer = 0.0f;
            EnemySpawnPoint();

        }
    }

    private void EnemySpawnPoint()
    {
        Vector2 randomPosition;

        
        if (Random.Range(0.0f, 1.0f) > 0.5f) // Spawn on the sides of the screen
        {
            randomPosition.y = Random.Range(-1.0f, 2.0f);

            if (Random.Range(0.0f, 1.0f) > 0.5f) // Spawn right
            {
                randomPosition.x = 2.0f;
            }
            else // Spawn left
            {
                randomPosition.x = -1.0f;
            }
        }
        else // Spawn on top/bottom of the screen
        {
            randomPosition.x = Random.Range(-1.0f, 2.0f);

            if (Random.Range(0.0f, 1.0f) > 0.5f) // Spawn top
            {
                randomPosition.y = 2.0f;
            }
            else // Spawn bottom
            {
                randomPosition.y = -1.0f;
            }
        }

        Vector2 spawnPos = Camera.main.ViewportToWorldPoint(randomPosition);
        Instantiate(tempEnemy, spawnPos, Quaternion.identity);
    }

}
