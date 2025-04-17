using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Weapon movement variable
    public Vector2 weaponDirection = new Vector2(0, 0);
    public Vector2 weaponVelocity;
    
    // Get reference of the player
    protected Player player;

    // Weapon Stats
    public List<WeaponStats> statistics;
    public List<WeaponUpgradeIncrement> upgradeIncrement;
    public int weaponLevel;
    protected int weaponDamage = 1;

    // Projectile Weapon Stats
    public bool isProjectile = false;
    public int weaponProjectiles = 1;
    public float weaponProjectileSpeed = 2.0f;

    // Melee Weapon Stats
    public bool isMelee = false;
    public float weaponLifeTime = 2.0f;

    private void Awake()
    {
        // Save reference of the player
        player = FindFirstObjectByType<Player>(); 
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Check player facing direction
        if (player.isFacingLeft)
        {
            weaponDirection.x = -1;
        }
        else
        {
            weaponDirection.x = 1;
        }
        Destroy(gameObject, weaponLifeTime);
    }
}

// base statistics of weapon
[System.Serializable]
public class WeaponStats
{
    public float damage, fireRate, areaSize, projectileCount, duration;
}

// statistics for weapon level up
[System.Serializable]
public class WeaponUpgradeIncrement
{
    public float damage, fireRate, areaSize, projectileCount, duration;
}
