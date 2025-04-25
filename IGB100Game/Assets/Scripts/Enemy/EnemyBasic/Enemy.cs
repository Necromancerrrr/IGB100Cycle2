using UnityEngine;

public class Enemy : EnemyStats
{

    Vector2 knockbackVelocity;
    float knockbackDuration;

    new void Awake()
    {
        base.Awake();
        // Locates player
    }
    
    void Update()
    {
        Movement();
        RespawnNearPlayer();
    }

    private void Movement()
    {
        if (knockbackDuration > 0) 
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime); // Constantly moves towards player
        }

        Vector2 lookDirection = (player.transform.position - transform.position).normalized;
        sr.flipX = lookDirection.x > 0;
    }

    public override void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce, float knockbackDuration = 0.2F)
    {
        base.TakeDamage(dmg, sourcePosition, knockbackForce, knockbackDuration);

        if (dmg > 0)
        {
            GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);
            
        }

        if (knockbackDuration > 0)
        {
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            Knockback(dir.normalized * knockbackForce, knockbackDuration);
        }

    }
    private void OnCollisionStay2D(Collision2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.gameObject.CompareTag("Player"))
        {
            //PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            col.gameObject.GetComponent<PlayerStats>().TakeDamage(currentDamage);
            //player.TakeDamage(currentDamage); // Make sure to use currentDamage instead of enemyData.Damage in case of any damage multipliers in the future
        }
    }

    public void Knockback(Vector2 velocity, float duration)
    {
        // Stops the knockback
        if (knockbackDuration > 0) { return; }

        // Begins knockback
        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
    
    public void RespawnNearPlayer()
    {
        if (Vector3.Distance (transform.position, player.transform.position) > 50) // check dist between enemy and player
        {
            Vector2 respawnPoint = RespawnPosition(1);
            
            transform.position = Camera.main.ViewportToWorldPoint(respawnPoint);
        }
    }
    
    public Vector2 RespawnPosition(float spawnDistance) // Really inefficient way to implement this because the code is already in BaseSpawner.cs BUT cant inherit multiple classes and dont wanna do it the proper way with interface wahtever
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
