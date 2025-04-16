using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public List<WeaponStats> statistics;
    public List<WeaponUpgradeIncrement> upgradeIncrement;
    public int weaponLevel;
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