using Unity.VisualScripting;
using UnityEngine;

public class EnemyChargerStats : EnemyStats
{
    public EnemyChargerScriptableObject chargerData;

    Vector2 knockbackVelocity;
    float knockbackDuration;
    // Current stats
    [HideInInspector]
    public float chargeFrequency;
    [HideInInspector]
    public float chargeSpeed;
    [HideInInspector]
    public float chargeDuration;
    [HideInInspector]
    public float chargeFreeze;

    // Charge logic
    public int ChargingPhase = 0;
    public float ChargeTimer = 5f;

    Vector2 target;
    Rigidbody2D rb;

    new void Awake()
    {
        base.Awake();
        // Charger stats
        chargeFrequency = chargerData.ChargeFrequency;
        chargeSpeed = chargerData.ChargeSpeed;
        chargeDuration = chargerData.ChargeDuration;
        chargeFreeze = chargerData.ChargeFreeze;

        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        ChargeUpdate();
        Movement();
    }

    private void Movement()
    {
        if (ChargingPhase == 0) // For all movement outside of the charge
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime); // Constantly moves towards player
        }

        // Sprite flips to play
        Vector2 lookDirection = (player.transform.position - transform.position).normalized;
        sr.flipX = lookDirection.x > 0;
    }
    private void ChargeUpdate()
    {
        ChargeTimer -= Time.deltaTime;
        if (ChargeTimer < 0 && ChargingPhase == 0) // Locks in the targetted position and freezes in place
        {
            ChargingPhase = 1;
            ChargeTimer = chargeDuration;
            target = new Vector2(player.transform.position.x, player.transform.position.y);
        }
        else if (ChargeTimer < 0 && ChargingPhase == 1) // Calculates the angle at which to move and starts charge towards the targetted position
        {
            ChargingPhase = 2;
            ChargeTimer = chargeFreeze;
            Vector2 angle = new Vector2(rb.transform.position.x - target.x, rb.transform.position.y - target.y).normalized;
            rb.linearVelocity = -angle * chargeSpeed;
            enemyAudio.PlayEnemyChargeSound();
        }
        else if (ChargeTimer < 0 && ChargingPhase == 2) // Charge ends, reset cooldown
        {
            ChargingPhase = 0;
            ChargeTimer = Random.Range(chargeFrequency * 0.9f, chargeFrequency * 1.1f);
            rb.linearVelocity = Vector2.zero;
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

    public override void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5, float knockbackDuration = 0.2F)
    {
        base.TakeDamage(dmg, sourcePosition, knockbackForce, knockbackDuration);

        if (knockbackDuration > 0)
        {
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            Knockback(dir.normalized * knockbackForce, knockbackDuration);
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
