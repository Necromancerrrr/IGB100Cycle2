using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    public Vector2 GetRandomPosition(float spawnDistance)
    {
        Vector2 randomPosition;

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

        return randomPosition;
    }
}
