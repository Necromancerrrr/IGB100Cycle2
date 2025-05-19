using System.Runtime.InteropServices;
using UnityEngine;

public class OrbBehaviour : ProjectileWeaponBehaviour
{
    // Components
    private GameObject target;
    private Rigidbody2D rb;
    private CircleCollider2D colli;
    [SerializeField] private ParticleSystem par;
    bool targetRand;

    // Orb logic
    float tickRate = 1f;
    float activeDuration = 0.1f;
    float pulseTimer = 10f;
    bool colliderActive = false;
    float burstTime = 0.05f;
    float burstParticles;
    float destroyTimer;

    [SerializeField] private AudioClip spawnAudio;

    override protected void Start()
    {
        weaponDamage = GetCurrentDamage();
        weaponSize = GetCurrentAreaSize();
        weaponDuration = GetCurrentDuration();
        destroyTimer = weaponDuration;
        pulseTimer = tickRate;
        rb = GetComponent<Rigidbody2D>();
        colli = GetComponent<CircleCollider2D>();
        colli.enabled = false;
        burstParticles = weaponSize * 15;
        SetEnemy();
        SetMovement();
        SetScale();
        transform.localScale = new Vector3(0, 0, 1);

        AudioManager.instance.PlaySFX(spawnAudio, transform, 1f);
    }

    public void targetSet(bool tar)
    {
        targetRand = tar;
    }

    // Update is called once per frame
    protected void Update()
    {
        destroyTimer -= Time.deltaTime;
        pulseTimer -= Time.deltaTime;
        burstTime += Time.deltaTime;
        if (destroyTimer <= 0)
        {
            transform.DetachChildren();
            par.transform.localScale = new Vector3(1, 1, 1);
            var emission = par.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] { }); // Intentionally empty
            Destroy(par, 1);
            Destroy(gameObject);
        }
        if (pulseTimer <= 0) 
        {
            Pulse();
            pulseTimer = tickRate;
        }
        if (pulseTimer <= tickRate - activeDuration && colliderActive == true)
        {
            colliderActive = false;
            colli.enabled = false;
        }

        // EASING STUFFS
        windDownTimer += Time.deltaTime;

        // Ease in on spawn
        if (transform.localScale != new Vector3(weaponSize, weaponSize, 1))
        {
            timeTakenUp = ScaleUpTransition(timeTakenUp, 1f, 0.5f);
        }

        // Ease out on death
        if (windDownTimer >= weaponDuration - 0.5)
        {
            timeTakenDown = ScaleDownTransition(timeTakenDown, 1f, 0.5f);
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
        else if (targetRand == true)
        {
            target = enemies[Random.Range(0, enemies.Length - 1)];
        }
        else if (targetRand ==  false)
        {
            target = enemies[0];
            GameObject player = GameObject.FindWithTag("Player");
            foreach (GameObject enemy in enemies)
            {
                if ((enemy.transform.position - player.transform.position).magnitude <= (target.transform.position - player.transform.position).magnitude)
                {
                    target = enemy;
                }
            }
        }
    }

    private void SetMovement() // Sends out projectile towards selected target
    {
        if(target != null)
        {
            Vector2 angle = rb.transform.position - target.transform.position;
            rb.linearVelocity -= angle.normalized * currentSpeed;
        }
    }

    private void SetScale() // Matches the scale of the collider and VFX to match area size
    {
        colli.radius = weaponSize;
        var shape = par.shape;
        shape.radius = weaponSize; // set to match area size
        var main = par.main;
        main.startSize = new ParticleSystem.MinMaxCurve(0.1f * 2, 0.2f * 2);
        main.startSpeed = -weaponSize;
    }

    override protected void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, -1 - weaponSize); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
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
