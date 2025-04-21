using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyProjectile : MonoBehaviour
{
    // Projectile stats
    float damage;
    float projectileSpeed;

    // Projectile logic
    float DespawnTimer = 5;

    // Components
    Rigidbody2D rb;
    Transform player;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
    }
    public void SetStats(float Damage, float speed) // This is called when the projectile is instantiated
    {
        damage = Damage;
        projectileSpeed = speed;
        FireProjectile(); // Ensures the projectile has the correct stats before firing
    }
    private void FireProjectile()
    {
        Vector2 target = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 angle = new Vector2(rb.transform.position.x - target.x, rb.transform.position.y - target.y).normalized;
        rb.linearVelocity = -angle * projectileSpeed;
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(damage); // Make sure to use currentDamage instead of enemyData.Damage in case of any damage multipliers in the future
            Destroy(gameObject);
        }
    }
    void Update() // Ensures the projectile doesn't become a memory leak
    {
        DespawnTimer -= Time.deltaTime;
        if (DespawnTimer <= 0) { Destroy(gameObject); }
    }
}
