using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Base script of all Projectile behaviours [To be placed on a prefab of a weapon that is a projectile i.e. an Arrow]
/// </summary>

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    protected Vector3 direction;

    // Current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;
    protected float currentAreaSize;
    protected float currentProjectileCount;
    [SerializeField] protected float currentDuration;

    // Stored numbers post modifier. USE THESE!!!
    protected float weaponDamage;
    protected float weaponSize;
    protected float weaponDuration;

    protected void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
        currentAreaSize = weaponData.AreaSize;
        currentProjectileCount = weaponData.ProjectileCount;
        currentDuration = weaponData.Duration;
    }

    public float GetCurrentDamage()
    {
        return currentDamage *= FindFirstObjectByType<PlayerStats>().CurrentMight;
    }
    public float GetCurrentAreaSize()
    {
        return currentAreaSize *= FindFirstObjectByType<PlayerStats>().CurrentAOE;
    }
    public float GetCurrentDuration()
    {
        return currentDuration *= FindFirstObjectByType<PlayerStats>().CurrentProjectileDuration;
    }
    protected virtual void Start()
    {
        weaponDamage = GetCurrentDamage();
        weaponSize = GetCurrentAreaSize();
        weaponDuration = GetCurrentDuration();
        Destroy(gameObject, weaponDuration);
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
            enemy.TakeDamage(weaponDamage, transform.position, 0); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
            ReducePierce(); 
        }
        else if (col.CompareTag("Prop"))
        {
            if(col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(weaponDamage);
                ReducePierce();
            }
        }
    }
    protected void ReducePierce() // Will destory the object after going through 'X' enemies
    {
        currentPierce--;
        if (currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
