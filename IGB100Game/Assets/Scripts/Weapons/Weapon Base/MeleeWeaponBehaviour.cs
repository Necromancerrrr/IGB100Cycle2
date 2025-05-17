using UnityEngine;

/// <summary>
/// Base script of all melee behaviours [To be placed on a prefab of a weapon that is melee i.e. a Sword]
/// </summary>
public class MeleeWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    // Current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;
    [SerializeField] protected float currentAreaSize;
    protected float currentProjectileCount;
    protected float currentDuration;

    // Stored numbers post modifier. USE THESE!!!
    protected float weaponDamage;
    protected float weaponSize;
    protected float weaponDuration;

    // scaling up and down transition vars
    public float windDownTimer;
    public float timeTakenUp = 0f;
    public float timeTakenDown = 0f;

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

    public float ScaleUpTransition(float timeTakenU, float scaleMax, float duration)
    {
        float t = timeTakenU / duration;
        transform.localScale = new Vector3(Mathf.Lerp(0, scaleMax, t), Mathf.Lerp(0, scaleMax, t), 1);
        //scaleSpeed += scaleRate;
        timeTakenU += Time.deltaTime;
        return timeTakenU;
    }

    public float ScaleDownTransition(float timeTakenD, float scaleMax, float duration)
    {
        float t = timeTakenD / duration;
        transform.localScale = new Vector3(Mathf.Lerp(scaleMax, 0, t), Mathf.Lerp(scaleMax, 0, t), 1);
        //scaleSpeed += scaleRate;
        timeTakenD += Time.deltaTime;
        return timeTakenD;
    }
    /*
    public float ScaleDownTransition(float scaleSpeed, float scaleRate, float scaleMax)
    {
        transform.localScale = new Vector3(Mathf.Lerp(scaleMax, 0, scaleSpeed), Mathf.Lerp(scaleMax, 0, scaleSpeed), 1);
        scaleSpeed += scaleRate;
        return scaleSpeed;
    }
    */
}