using System.Collections.Generic;
using UnityEngine;

// Weapon holds all the base stats of the weapon type
// All weapons are children of Weapon, so can reference parts of this script
// Weapon behaviour is not called here. Those are in each weapon's respective Weapon script
public class Weapon : MonoBehaviour
{
    // Weapon movement variable
    public Vector2 weaponDirection = new Vector2(0, 0); // Direction of object spawning
    public Vector2 weaponVelocity;
    
    // Get reference of the player
    protected Player player;

    // Weapon Stats
    public List<WeaponStats> statistics;
    public List<WeaponUpgradeIncrement> upgradeIncrement;
    public int weaponLevel;
    protected int weaponDamage = 1;

    // Projectile Weapon Stats
    public bool isProjectile = false; // not used as of now
    public int weaponProjectiles = 1;
    public float weaponProjectileSpeed = 2.0f;

    // Melee Weapon Stats
    public bool isMelee = false; // not used as of now
    public float weaponLifeTime = 2.0f;

    private void Awake()
    {
        // Save reference of the player
        player = FindFirstObjectByType<Player>(); 
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
