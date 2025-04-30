using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SeekerBehaviour : ProjectileWeaponBehaviour 
{
    // Components
    private GameObject target;
    private Rigidbody2D rb;
    [SerializeField] private GameObject par;
    override protected void Start()
    {
        // Pulls stats and finds components
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        SetEnemy();
    }
    protected void Update()
    {
        Movement();
    }
    private void Movement() // If there is no target, find one. Otherwise, move towards the current target
    {
        if (target == null)
        {
            SetEnemy();
        }
        else
        {
            Vector2 angle = rb.transform.position - target.transform.position;
            rb.linearVelocity -= angle.normalized * currentSpeed * Time.deltaTime;
        }
    }
    private void SetEnemy() // Selects a random enemy as the target. If there are no valid targets, self destruct
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length <= 0) { Destroy(gameObject); }
        else
        {
            target = enemies[Random.Range(0, enemies.Length - 1)];
        }
    }
    override protected void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, 0); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
            PlayVFX();
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(weaponDamage);
                PlayVFX();
            }
        }
    }
    protected void OnDestroy()
    {
        PlayVFX();
    }
    void PlayVFX() // Plays a little VFX
    {
        GameObject particle = Instantiate(par);
        particle.transform.position = transform.position;
        Destroy(particle, 1f);
    }
}
