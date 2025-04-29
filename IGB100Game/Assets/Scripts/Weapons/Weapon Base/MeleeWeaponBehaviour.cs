using UnityEngine;

/// <summary>
/// Base script of all melee behaviours [To be placed on a prefab of a weapon that is melee i.e. a Sword]
/// </summary>
public class MeleeWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    public float destoryAfterSeconds;

    // Current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;
    [SerializeField] protected float currentAreaSize;
    protected float currentProjectileCount;
    protected float currentDuration;

    protected float weaponDamage;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
        currentAreaSize = weaponData.AreaSize;
        currentProjectileCount = weaponData.ProjectileCount;
        currentDuration = weaponData.Duration;
        destoryAfterSeconds = weaponData.Duration;
    }

    public float GetCurrentDamage()
    {
        return currentDamage *= FindFirstObjectByType<PlayerStats>().CurrentMight;
    }

    protected virtual void Start()
    {
        weaponDamage = GetCurrentDamage();
        Destroy(gameObject, destoryAfterSeconds);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, 0); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(weaponDamage);
            }
        }
    }
}
