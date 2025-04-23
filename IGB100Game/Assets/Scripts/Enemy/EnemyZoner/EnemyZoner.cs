using UnityEngine;

public class EnemyZonerStats : EnemyStats
{
    public EnemyZonerScriptableObject zonerData;

    Vector2 knockbackVelocity;
    float knockbackDuration;

    // Current stats
    [HideInInspector]
    public float zoneFrequency;
    [HideInInspector]
    public float zoneCount;
    [HideInInspector]
    public float zoneSize;
    [HideInInspector]
    public float zoneDelay;

    // Zoner logic
    float zoneTimer = 5;

    [SerializeField] GameObject AOE;

    new void Awake()
    {
        base.Awake();
        // Shooter Stats
        zoneFrequency = zonerData.ZoneFrequency;
        zoneCount = zonerData.ZoneCount;
        zoneSize = zonerData.ZoneSize;
        zoneDelay = zonerData.ZoneDelay;

    }
    void Update()
    {
        Movement();
        AOEUpdate();
    }
    private void Movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime); // Constantly moves towards player

        //Sprite flips towards player
        Vector2 lookDirection = (player.transform.position - transform.position).normalized;
        sr.flipX = lookDirection.x > 0;
    }
    private void AOEUpdate()
    {
        zoneTimer -= Time.deltaTime;
        if (zoneTimer <= 0)
        {
            for (int i = 0; i < zoneCount; i++)
            {
                SpawnAOE();
            }
            zoneTimer = zoneFrequency;
        }
    }
    private void SpawnAOE()
    {
        GameObject instance = Instantiate(AOE);
        instance.transform.position = transform.position; // Assign the position to be the same as this object which is parented to the player
        instance.GetComponent<EnemyAOE>().SetStats(currentDamage, zoneSize, zoneDelay);
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
