using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class VampiricBladeBehaviour : MeleeWeaponBehaviour
{
    float Timer = 1.4f;
    bool clockwise;
    float projectileCount;
    [SerializeField] private GameObject col;
    [SerializeField] private GameObject projPoint;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject indicator;
    private GameObject player;
    float angle;
    float nextShot;

    // Heal Logic
    float heal = 0;
    bool healed = false;

    // Visual Feedback Par
    [SerializeField] private Color parColour;
    [SerializeField] private GameObject par;
    bool shakeBool = false;

    [SerializeField] private AudioClip spawnAudio;

    override protected void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");
        if (Random.Range(0, 2)  == 0) // Randomly generates direction
        { 
            clockwise = true;
            indicator.transform.localPosition = new Vector3(0.3f, 0.5f, 0);
            indicator.GetComponent<SpriteRenderer>().flipX = false;
        } 
        else 
        { 
            clockwise = false;
            indicator.transform.localPosition = new Vector3(-0.3f, 0.5f, 0);
            indicator.GetComponent<SpriteRenderer>().flipX = true;
        }
        projectileCount = Mathf.Round(weaponData.ProjectileCount * FindFirstObjectByType<PlayerStats>().CurrentProjectileCount);
        SetScale();
        SetRotation();

        AudioManager.instance.PlaySFX(spawnAudio, transform, 1f);
    }

    void SetScale()
    {
        col.transform.localScale = new Vector3(0, 0, 1);
        col.GetComponent<CapsuleCollider2D>().enabled = true;
    }

    void SetRotation() // Finds the closest enemy and takes their position. Converts that into an angle, then prepares for swing
    {
        Vector2 target;
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemyList.Length > 0)
        {
            target = enemyList[0].transform.position;
            foreach (GameObject enemy in enemyList) // Search through every enemy
            {
                if ((enemy.transform.position - player.transform.position).magnitude <= ((Vector3)target - player.transform.position).magnitude)
                {
                    target = enemy.transform.position; // And if they are closer than the current target, swap them in
                }
            }
            Vector2 angleVec = ((Vector2)transform.position - target).normalized; // Converts the enemy into a vector
            if (clockwise == true) 
            { 
                angle = Mathf.Atan2(angleVec.y, angleVec.x) * Mathf.Rad2Deg + 150; // Sets position based on direction of swing
                nextShot = angle - 120 / projectileCount; // Sets up projectile shooting
            }
            else if (clockwise == false) 
            { 
                angle = Mathf.Atan2(angleVec.y, angleVec.x) * Mathf.Rad2Deg + 30; // Sets position based on direction of swing
                nextShot = angle + 120 / projectileCount; // Sets up projectile shooting
            }
            
        }
        else { Destroy(gameObject); } // Destroy object if no enemies exist
    }

    private void Update()
    {
        gameObject.transform.position = player.transform.position; // Ensures the sword stays around the player
        Timer -= Time.deltaTime;
        if (Timer <= 0.7 && Timer >= 0.5) // Hard coded values for swinging the sword
        {
            Destroy(indicator);
            if (clockwise == true)
            {
                angle -= Time.deltaTime * 600;
            }
            else if (clockwise == false)
            {
                angle += Time.deltaTime * 600;
            }
            projectileCheck();
            if (shakeBool == false) // Sets the screen shake
            {
                shakeBool = true;
                GameObject.FindWithTag("CineCamera").GetComponent<ScreenShake>().SetShake(weaponSize * 3f, 0.4f);
            }
        }
        if (Timer <= 0.5 && healed == false)
        {
            player.GetComponent<PlayerStats>().RestoreHealth(heal);
            healed = true;
        }
        gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        if (Timer <= 0) { Destroy(gameObject); }

        
        // EASING STUFFS
        windDownTimer += Time.deltaTime;

        // Ease in on spawn
        if (transform.localScale != new Vector3(weaponSize, weaponSize, 1))
        {
            float t = timeTakenUp / 0.5f;
            col.transform.localScale = new Vector3(Mathf.Lerp(0, weaponSize, t), Mathf.Lerp(0, weaponSize, t), 1);
            timeTakenUp += Time.deltaTime;
        }
        
        // Ease out on death
        if (Timer <= 0.3)
        {
            float t = timeTakenDown / 0.2f;
            col.transform.localScale = new Vector3(Mathf.Lerp(weaponSize, 0, t), Mathf.Lerp(weaponSize, 0, t), 1);
            timeTakenDown += Time.deltaTime;
        }
        
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
        projInstance.transform.rotation = transform.rotation;
        projInstance.GetComponent<VampiricProjectileBehaviour>().SetStats(weaponDamage, weaponSize, currentSpeed);
    }

    new protected void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, weaponSize/3 + 2); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
            heal += 1;
            GameObject parInstance = Instantiate(par);
            parInstance.GetComponent<HitParticle>().SetValues(transform.position, col.transform.position, parColour, 0.5f);
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(weaponDamage);
            }
            GameObject parInstance = Instantiate(par);
            parInstance.GetComponent<HitParticle>().SetValues(transform.position, col.transform.position, parColour, 0.5f);
        }
    }
}
