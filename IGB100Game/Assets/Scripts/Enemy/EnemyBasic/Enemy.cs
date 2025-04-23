using UnityEngine;

public class Enemy : EnemyStats
{
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
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime); // Constantly moves towards player
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
}
