using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class EnemyStats : MonoBehaviour
{
    public bool isBoss;
    private BossHealthUI healthBar;

    public EnemyScriptableObject enemyData;

    // Reference the player
    [HideInInspector]
    public PlayerStats player;

    [HideInInspector]
    public bool isDebuffed = false;

    [HideInInspector]
    public float debuffTimer;

    public float baseMovespeed;

    // Current stats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    [HideInInspector] 
    public float knockbackModifier;


    [HideInInspector]
    public EnemyAudio enemyAudio;
    private GameObject audioManager;

    // Knockback logic
    protected Vector2 knockbackVelocity;
    protected float knockbackDuration;

    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1);
    public float damageFlashDuration = 0.2f;
    public float deathFadeTime = 0.6f;
    Color originalColor;
    protected SpriteRenderer sr;
    protected Collider2D enemyCollider;
    
    [SerializeField] private GameObject deathParticleSystem;

    protected void Awake()
    {
        // Enemy base stats
        currentMoveSpeed = enemyData.MoveSpeed;
        baseMovespeed = currentMoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        knockbackModifier = enemyData.KnockbackMod;
        player = FindFirstObjectByType<PlayerStats>();
        sr = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
        originalColor = sr.color;
        enemyAudio = GetComponent<EnemyAudio>();
        
        audioManager = GameObject.FindWithTag("AudioManager");
        
        if (isBoss == true)
        {
            healthBar = GetComponentInChildren<BossHealthUI>();
            healthBar.SetHealthMax(enemyData.MaxHealth);
            healthBar.SetHealthValue(currentHealth);
        }
    }

    protected void Update()
    {
        if (isDebuffed)
        {
            debuffTimer -= Time.deltaTime;

            if (debuffTimer <= 0)
            {
                currentMoveSpeed = baseMovespeed;
                isDebuffed = false;
            }
        }
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        
    }
    IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
    }
    public virtual void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce, float knockbackDuration = 0.2f)
    {
        currentHealth -= dmg;
        if (isBoss == true)
        {
            healthBar.SetHealthValue(currentHealth);
        }
        StartCoroutine(DamageFlash());

        //enemyAudio.PlayEnemyHurtSound();
        audioManager.GetComponent<AudioManager>().enemyHurtQueue += 1;

        if (dmg > 0)
        {
            GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);
        }
        if (knockbackDuration * knockbackModifier > 0)
        {
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            Knockback(dir.normalized * knockbackForce * knockbackModifier, knockbackDuration * knockbackModifier);
        }
        if (currentHealth <= 0)
        {
            Kill();
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
    public void Kill()
    {
        enemyCollider.enabled = false;
        currentMoveSpeed = 0;
        player.CurrentKills++;
        StartCoroutine(KillFade());
    }

    public void ApplySlow(float duration, float percentSlow)
    {
        if (!isDebuffed)
        {
            Debug.Log("Apply slow");
            debuffTimer = duration;
            currentMoveSpeed *= percentSlow;
            isDebuffed = true;
        }
    }

    public void RespawnNearPlayer()
    {
        //Vector3.Distance (transform.position, player.transform.position) > 25
        if (Mathf.Abs(player.transform.position.x - transform.position.x) > 25 || Mathf.Abs(player.transform.position.y - transform.position.y) > 15) // check dist between enemy and player
        {
            Vector2 respawnPoint = RespawnPosition(0.6f);
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

    IEnumerator KillFade()
    {
        currentMoveSpeed = 0f;
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, originalAlpha = sr.color.a;
        Instantiate(deathParticleSystem, transform.position, Quaternion.identity);
        while(t < deathFadeTime)
        {
            yield return w;
            t+= Time.deltaTime;

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1 - t/ deathFadeTime) * originalAlpha);
        }

        Destroy(gameObject);
        
    }
}
