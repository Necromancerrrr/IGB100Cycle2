using UnityEngine;

public abstract class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    
    // Reference the player
    [HideInInspector]
    public PlayerStats player;

    // Current stats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    protected void Awake()
    {
        // Enemy base stats
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        player = FindFirstObjectByType<PlayerStats>();
    }
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if (dmg > 0)
        {
            GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);
        }

        if (currentHealth <= 0)
        {
            Kill();
        }
    }
    public void Kill()
    {
        Destroy(gameObject);
        player.CurrentKills++;
    }
}
