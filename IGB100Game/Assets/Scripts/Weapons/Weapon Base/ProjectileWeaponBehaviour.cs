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

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;


        // Rotation of projecitles depending on player movement direction
        float dirX = direction.x;
        float dirY = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if (dirX < 0 && dirY == 0)
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * 1;
        }
        else if (dirX == 0 && dirY < 0) // up
        {
            rotation.z = -90f;
        }
        else if (dirX == 0 && dirY > 0) // down
        {
            rotation.z = 90f;
        }
        else if (dir.x > 0 && dir.y > 0) // right up
        {
            rotation.z = 45f;
        }
        else if(dir.x > 0 && dir.y <0) // right down
        {
            rotation.z = -45f;
        }
        else if(dir.x < 0 && dir.y > 0)
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * 1;
            rotation.z = -45f;
        }
        else if(dir.x < 0 && dir.y < 0)
        {
            scale.x = scale.x * -1;
            scale.y= scale.y * 1;
            rotation.z = 45f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
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
    /*
    public float ScaleUpTransition(float scaleSpeed, float scaleRate, float scaleMax)
    {
        transform.localScale = new Vector3(Mathf.Lerp(0, scaleMax, scaleSpeed), Mathf.Lerp(0, scaleMax, scaleSpeed), 1);
        scaleSpeed += scaleRate;
        return scaleSpeed;
    }

    public float ScaleDownTransition(float scaleSpeed, float scaleRate, float scaleMax)
    {
        transform.localScale = new Vector3(Mathf.Lerp(scaleMax, 0, scaleSpeed), Mathf.Lerp(scaleMax, 0, scaleSpeed), 1);
        scaleSpeed += scaleRate;
        return scaleSpeed;
    }

    */

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

    
}
