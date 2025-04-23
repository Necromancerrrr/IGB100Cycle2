using UnityEngine;

public class Reaper : EnemyStats
{
    // Reaper Logic
    float SpeedIncreaseTimer = 5;
    new void Awake()
    {
        base.Awake();
    }
    void Update()
    {
        Movement();
        SpeedIncrease();
    }
    private void Movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime); // Constantly moves towards player
    }
    private void SpeedIncrease()
    {
        SpeedIncreaseTimer -= Time.deltaTime;
        if (SpeedIncreaseTimer < 0)
        {
            currentMoveSpeed += 1;
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
}
