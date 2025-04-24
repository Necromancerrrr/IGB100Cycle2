using UnityEngine;

/// <summary>
/// Base script of all Projectile behaviours [To be placed on a prefab of a weapon that is a projectile i.e. an Arrow]
/// </summary>

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    protected Vector3 direction;
    public float destroyAfterSeconds;

    // Current stats
    [SerializeField] protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;
    protected float currentAreaSize;
    protected float currentProjectileCount;
    protected float currentDuration;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
        currentAreaSize = weaponData.AreaSize;
        currentProjectileCount = weaponData.ProjectileCount;
        currentDuration = weaponData.Duration;
        destroyAfterSeconds = currentDuration;
    }

    public float GetCurrentDamage()
    {
        return currentDamage *= FindFirstObjectByType<PlayerStats>().CurrentMight;
    }
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }


    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage(),transform.position, 0); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
            ReducePierce(); 
        }
        else if (col.CompareTag("Prop"))
        {
            if(col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(GetCurrentDamage());
                ReducePierce();
            }
        }
    }

    void ReducePierce() // Will destory the object after going through 'X' enemies
    {
        currentPierce--;
        if (currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
