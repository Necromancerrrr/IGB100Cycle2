using UnityEngine;

public class Enemy : EnemyStats
{
    new void Update()
    {
        base.Update();
        Movement();
    }
    private void Movement()
    {
        if (knockbackDuration <= 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime); // Constantly moves towards player
        }
        Vector2 lookDirection = (player.transform.position - transform.position).normalized;
        sr.flipX = lookDirection.x > 0;
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
    
}
