using UnityEngine;

public class EnemyShooterStats : EnemyStats
{
    public EnemyShooterScriptableObject shooterData;

    Vector2 knockbackVelocity;
    float knockbackDuration;
    // Current stats
    [HideInInspector]
    public float firingFrequency;
    [HideInInspector]
    public float projectileCount;
    [HideInInspector]
    public float projectileInterval;
    [HideInInspector]
    public float projectileSpeed;

    // Shooting Logic
    float Ammo;
    float ShootTimer = 5;
    public float MovementTimer = 0;

    Rigidbody2D rb;
    [SerializeField] GameObject projectile;

    new void Awake()
    {
        base.Awake();
        // Shooter Stats
        firingFrequency = shooterData.FiringFrequency;
        projectileCount = shooterData.ProjectileCount;
        projectileInterval = shooterData.ProjectileInterval;
        projectileSpeed = shooterData.ProjectileSpeed;

        Ammo = projectileCount;
        rb = player.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Movement();
        ShootUpdate();
    }
    private void Movement()
    {
        MovementTimer -= Time.deltaTime;
        if (new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).magnitude >= 10f) // If the shooter is a significant distance away
        {
            rb.linearVelocity = Vector2.zero;
            MovementTimer = 0;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime); // Constantly moves towards player
        }
        else
        {
            if (MovementTimer <= 0)
            {
                Vector2 angle = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                rb.linearVelocity = angle * Random.Range(currentMoveSpeed * 0.8f, currentMoveSpeed * 1.2f);
                MovementTimer = 3;
            }
        }
    }
    private void ShootUpdate()
    {
        ShootTimer -= Time.deltaTime;
        if (ShootTimer < 0 && Ammo <= 1) 
        {
            ShootTimer = Random.Range(firingFrequency * 0.9f, firingFrequency * 1.1f);
            Ammo = projectileCount;
            SpawnProjectile();
        }
        else if (ShootTimer < 0 && Ammo > 1)
        {
            ShootTimer = projectileInterval;
            Ammo -= 1;
            SpawnProjectile();
        }
    }
    private void SpawnProjectile()
    {
        GameObject instance = Instantiate(projectile);
        instance.transform.position = transform.position; // Assign the position to be the same as this object which is parented to the player
        instance.GetComponent<EnemyProjectile>().SetStats(currentDamage, projectileSpeed);
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
