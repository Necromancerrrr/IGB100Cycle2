using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
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
    [HideInInspector]
    public EnemyAudio enemyAudio;


    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1);
    public float damageFlashDuration = 0.2f;
    public float deathFadeTime = 0.6f;
    Color originalColor;
    protected SpriteRenderer sr;
    protected Collider2D enemyCollider;
    

    protected void Awake()
    {
        // Enemy base stats
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        player = FindFirstObjectByType<PlayerStats>();
        sr = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
        originalColor = sr.color;
        enemyAudio = GetComponent<EnemyAudio>();
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
        StartCoroutine(DamageFlash());
        enemyAudio.PlayEnemyHurtSound();

        Debug.Log(dmg);

        if (currentHealth <= 0)
        {
            Kill();
        }
    }
    public void Kill()
    {
        enemyCollider.enabled = false;
        currentMoveSpeed = 0;
        player.CurrentKills++;
        StartCoroutine(KillFade());
    }

    IEnumerator KillFade()
    {
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, originalAlpha = sr.color.a;

        while(t < deathFadeTime)
        {
            yield return w;
            t+= Time.deltaTime;

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1 - t/ deathFadeTime) * originalAlpha);
        }

        Destroy(gameObject);
    }
}
