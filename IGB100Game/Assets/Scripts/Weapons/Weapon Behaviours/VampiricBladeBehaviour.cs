using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VampiricBladeBehaviour : MeleeWeaponBehaviour
{
    float Timer = 4.2f;
    bool clockwise;
    float projectileCount;
    [SerializeField] private GameObject col;
    [SerializeField] private GameObject projPoint;
    [SerializeField] private GameObject projectile;
    private GameObject player;
    float angle;
    float nextShot;
    // Update is called once per frame
    override protected void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");
        if (Random.Range(0, 2)  == 0) { clockwise = true; } // Randomly generates direction
        else { clockwise = false; }
        projectileCount = Mathf.Round(weaponData.ProjectileCount * FindFirstObjectByType<PlayerStats>().CurrentProjectileCount);
        SetScale();
        SetRotation();
    }
    void SetScale()
    {
        col.transform.localScale = new Vector2(weaponSize, weaponSize);
        col.GetComponent<CapsuleCollider2D>().enabled = true;
    }
    void SetRotation() // Finds the closest enemy and takes their position. Converts that into an angle, then prepares for swing
    {
        Vector2 target;
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemyList.Length > 0)
        {
            target = enemyList[0].transform.position;
            foreach (GameObject enemy in enemyList)
            {
                if ((enemy.transform.position - player.transform.position).magnitude <= ((Vector3)target - player.transform.position).magnitude)
                {
                    target = enemy.transform.position;
                }
            }
            Vector2 angleVec = ((Vector2)transform.position - target).normalized;
            if (clockwise == true) 
            { 
                angle = Mathf.Atan2(angleVec.y, angleVec.x) * Mathf.Rad2Deg - 180;
                nextShot = angle - 120 / projectileCount;
            }
            else if (clockwise == false) 
            { 
                angle = Mathf.Atan2(angleVec.y, angleVec.x) * Mathf.Rad2Deg;
                nextShot = angle + 120 / projectileCount;
            }
            
        }
        else { Destroy(gameObject); }
    }
    private void Update()
    {
        gameObject.transform.position = player.transform.position;
        Timer -= Time.deltaTime;
        if (Timer <= 2.2 && Timer >= 2)
        {
            if (clockwise == true)
            {
                angle -= Time.deltaTime * 600;
            }
            else if (clockwise == false)
            {
                angle += Time.deltaTime * 600;
            }
            projectileCheck();
        }
        gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        if (Timer <= 0) { Destroy(gameObject); }
    }
    void projectileCheck() // Checks if the next projectile should have fired already
    {
        if (nextShot >= angle && clockwise == true)
        {
            projectileFire();
            nextShot -= 120 / projectileCount;
        }
        if (nextShot <= angle && clockwise == false)
        {
            projectileFire();
            nextShot += 120 / projectileCount;
        }
    }
    void projectileFire()
    {
        GameObject projInstance = Instantiate(projectile);
        projInstance.transform.position = projPoint.transform.position;
        projInstance.GetComponent<VampiricProjectileBehaviour>().SetStats(weaponDamage, weaponSize, angle, currentSpeed);
    }
    new protected void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, weaponSize/3 + 3); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
            // Insert heal here
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
