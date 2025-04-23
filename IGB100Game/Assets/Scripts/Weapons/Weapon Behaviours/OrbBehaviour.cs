using System.Runtime.InteropServices;
using UnityEngine;

public class OrbBehaviour : ProjectileWeaponBehaviour
{
    // Components
    private GameObject target;
    private Rigidbody2D rb;
    private CircleCollider2D colli;
    [SerializeField] private ParticleSystem par;

    // Orb logic
    float tickRate = 1f;
    float activeDuration = 0.1f;
    float pulseTimer = 10f;
    bool colliderActive = false;
    float burstTime = 0.05f;
    float burstParticles = 30;
    override protected void Start()
    {
        base.Start();
        pulseTimer = tickRate;
        rb = GetComponent<Rigidbody2D>();
        colli = GetComponent<CircleCollider2D>();
        colli.enabled = false;
        SetEnemy();
        SetMovement();
        SetScale();
    }
    // Update is called once per frame
    protected void Update()
    {
        pulseTimer -= Time.deltaTime;
        burstTime += Time.deltaTime;
        if (pulseTimer <= 0) 
        {
            Debug.Log("pulse");
            Pulse();
            pulseTimer = tickRate;
        }
        if (pulseTimer <= tickRate - activeDuration && colliderActive == true)
        {
            colliderActive = false;
            colli.enabled = false;
        }
    }
    private void Pulse() // Activates the orb's collider and plays appropriate VFX.
    {
        colliderActive = true;
        colli.enabled = true;
        var emission = par.emission;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(burstTime, burstParticles) });
    }
    private void SetEnemy() // Selects a random enemy as the target. If there are no valid targets, self destruct.
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length <= 0) { Destroy(gameObject); }
        else
        {
            target = enemies[Random.Range(0, enemies.Length - 1)];
        }
    }
    private void SetMovement() // Sends out projectile towards selected target
    {
        Vector2 angle = rb.transform.position - target.transform.position;
        rb.linearVelocity -= angle.normalized * currentSpeed;
    }
    private void SetScale() // Matches the scale of the collider and VFX to match area size
    {
        colli.radius = currentAreaSize;
        var shape = par.shape;
        shape.radius = currentAreaSize; // set to match area size
        var main = par.main;
        main.startSize = new ParticleSystem.MinMaxCurve(0.1f * 2, 0.2f * 2);
    }
    override protected void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage(), transform.position, 0f); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(GetCurrentDamage());
            }
        }
    }
}
