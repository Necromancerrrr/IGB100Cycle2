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

    // Components
    Transform player;
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
        // Locates player
        player = GameObject.FindWithTag("Player").transform;
    }
    void Update()
    {
        Movement();
        ShootUpdate();
    }
    private void Movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime); // Constantly moves towards player
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
}
