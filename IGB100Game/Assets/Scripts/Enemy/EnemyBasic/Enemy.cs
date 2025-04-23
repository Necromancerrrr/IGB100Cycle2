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

    public override void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5, float knockbackDuration = 0.2F)
    {
        base.TakeDamage(dmg, sourcePosition, knockbackForce, knockbackDuration);
        
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
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage); // Make sure to use currentDamage instead of enemyData.Damage in case of any damage multipliers in the future
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
}
