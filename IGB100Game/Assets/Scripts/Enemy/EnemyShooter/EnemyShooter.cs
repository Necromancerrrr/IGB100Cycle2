using UnityEngine;

public class EnemyShooterStats : EnemyStats
{
    public EnemyShooterScriptableObject shooterData;
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
    new void Update()
    {
        base.Update();
        Movement();
        ShootUpdate();
        RespawnNearPlayer();
    }
    private void Movement()
    {
        if (knockbackDuration <= 0)
        {
            if (new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).magnitude >= 10f) // If the shooter is a significant distance away
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime); // Constantly moves towards player
            }
            else if (new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).magnitude <= 3f) // If the shooter is too close
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -1 * currentMoveSpeed * Time.deltaTime); // Constantly moves away from the player
            }
            //Sprite flips towards player
            Vector2 lookDirection = (player.transform.position - transform.position).normalized;
            sr.flipX = lookDirection.x > 0;
        }
    }
    private void ShootUpdate()
    {
        ShootTimer -= Time.deltaTime;
        if (ShootTimer < 0 && Ammo <= 0 && new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).magnitude >= 3f)
        { // Prepares to shoot if player has no ammo and is far enough away
            ShootTimer = projectileInterval;
            Ammo = projectileCount;
        }
        else if (ShootTimer < 0 && Ammo >= 1) // Fires projectile
        {
            Ammo -= 1;
            SpawnProjectile();
            if (Ammo >= 1) // If there is at least one ammo left, prepare to shoot again
            {
                ShootTimer = projectileInterval;
            }
            else // Set shooting on cooldown
            {
                ShootTimer = Random.Range(firingFrequency * 0.9f, firingFrequency * 1.1f);
            }
        }
    }
    private void SpawnProjectile()
    {
        GameObject instance = Instantiate(projectile);
        instance.transform.position = transform.position; // Assign the position to be the same as this object which is parented to the player
        instance.GetComponent<EnemyProjectile>().SetStats(currentDamage, projectileSpeed);
        enemyAudio.PlayEnemyShootSound();
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
