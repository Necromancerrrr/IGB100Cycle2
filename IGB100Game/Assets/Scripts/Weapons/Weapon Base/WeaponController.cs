using UnityEngine;

/// <summary>
/// Base script for all weapon controllers
/// <summary>

public class WeaponController : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    float currentCooldown;
    float currentProjectileCount;

    protected PlayerMovement pm;

    [SerializeField] protected float weaponCooldown;
    [SerializeField] protected float weaponCount;
    public float GetCurrentCooldown()
    {
        return currentCooldown / FindFirstObjectByType<PlayerStats>().CurrentCDR; //* FindFirstObjectByType<PlayerStats>().CurrentCooldown;
    }
    public float GetCurrentProjectileCount()
    {
        return Mathf.Round(currentProjectileCount * FindFirstObjectByType<PlayerStats>().CurrentProjectileCount);
    }

    protected virtual void Start()
    {
        pm = FindFirstObjectByType<PlayerMovement>();
        Attack();
        //recalculateStats();
    }
    void recalculateStats()
    {
        currentCooldown = weaponData.CooldownDuration; // At the start, set current cooldown to the cooldown duration;
        currentProjectileCount = weaponData.ProjectileCount; // In case you need to repeat certain commands (instantiates)
        weaponCooldown = GetCurrentCooldown();
        weaponCount = GetCurrentProjectileCount();
    }

    protected virtual void Update()
    {
        weaponCooldown -= Time.deltaTime;
        if (weaponCooldown <= 0f) 
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        recalculateStats();
    }
}
