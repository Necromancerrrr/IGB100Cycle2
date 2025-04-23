using UnityEngine;

/// <summary>
/// Base script for all weapon controllers
/// <summary>

public class WeaponController : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    [SerializeField] float currentCooldown;
    [SerializeField] protected float currentProjectileCount;

    protected PlayerMovement pm;

    protected virtual void Start()
    {
        pm = FindFirstObjectByType<PlayerMovement>();
        currentCooldown = weaponData.CooldownDuration; // At the start, set current cooldown to the cooldown duration;
        currentProjectileCount = Mathf.Round(weaponData.ProjectileCount); // In case you need to repeat certain commands (instantiates)
    }

    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f) 
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration; // Reset cooldown
    }
}
