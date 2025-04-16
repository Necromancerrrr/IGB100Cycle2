using System.Runtime.CompilerServices;
using UnityEngine;

public class Orb : EquipmentAbstract
{
    // Orb specific values
    [SerializeField] private GameObject orb;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileVariance;

    // Scaleables
    [SerializeField] private float baseDamage;
    [SerializeField] private float levelDamage;
    [SerializeField] private float baseFireRate;
    [SerializeField] private float levelFireRate;
    [SerializeField] private float baseAOESize;
    [SerializeField] private float levelAOESize;
    [SerializeField] private float baseProjectileCount;
    [SerializeField] private float levelProjectileCount;
    [SerializeField] private float baseDuration;
    [SerializeField] private float levelDuration;

    private float distance;
    // 1. Finds every enemy, then stores the position of the closest one.
    // 2. Creates an instance for every projectile count (rounded), then slightly randomizes angle and adds velocity
    // 3. Passes on damage, AOE size and duration stats onto each instance
    // 4. Returns fire rate back to player for that script to track when to fire again
    override public float WeaponCall(float DamageLevel, float FireRateLevel, float AOESizeLevel, float ProjectileCountLevel, float DurationLevel)
    {
        GameObject[] EnemyList = GameObject.FindGameObjectsWithTag("Enemy");
        if (EnemyList.Length > 0) // Ensures that there are in fact enemies to target
        {
            distance = 9999999f; Vector2 target = Vector2.zero;
            foreach (GameObject enemy in EnemyList)
            {
                Vector2 diffCalc = player.GetComponent<Rigidbody2D>().position - enemy.GetComponent<Rigidbody2D>().position;
                if (diffCalc.magnitude < distance)
                {
                    target = enemy.GetComponent<Rigidbody2D>().position;
                }
            }
            for (int i = 0; i < Mathf.Round(baseProjectileCount + levelProjectileCount * ProjectileCountLevel); i++)
            {
                GameObject orbInstance = Instantiate(orb);
                Vector2 angleCalc = player.GetComponent<Rigidbody2D>().position - target;
                //angleCalc.x += (Mathf.PerlinNoise(angleCalc.x, angleCalc.y) - 0.5f) * projectileVariance;
                //angleCalc.y += (Mathf.PerlinNoise(angleCalc.y, angleCalc.x) - 0.5f) * projectileVariance;
                orb.GetComponent<Rigidbody2D>().linearVelocity = angleCalc.normalized * projectileSpeed;
                orbInstance.GetComponent<OrbProjectile>().OnInstantiate(baseDamage + levelDamage * DamageLevel, baseAOESize + levelAOESize * AOESizeLevel, baseDuration + levelDuration * DurationLevel);
            }
        }
        return baseFireRate - levelFireRate * FireRateLevel;
    }
}
