using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class EnemyAOE : MonoBehaviour
{
    // Zone stats
    float damage;
    float travelSpeed;
    float zoneSize;
    float zoneDelay;

    // Zone logic
    float zoneTimer = 5;
    bool active = false;
    bool targetReached = false;

    // Components
    Transform player;
    CircleCollider2D colli;
    ParticleSystem trail;
    [SerializeField] ParticleSystem par;
    [SerializeField] ParticleSystem sub;
    Vector2 target;
    void Awake() // Gets components and disables irrelevant ones for now
    {
        colli = GetComponent<CircleCollider2D>();
        colli.enabled = false;
        trail = GetComponent<ParticleSystem>();
        par.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        player = GameObject.FindWithTag("Player").transform;
    }
    public void SetStats(float Damage, float Speed, float Size, float Delay) // This is called when the projectile is instantiated
    {
        damage = Damage;
        travelSpeed = Speed;
        zoneSize = Size;
        zoneDelay = Delay;
        zoneTimer = zoneDelay;
        SetPosition();
        SetScale();
        par.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        sub.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
    void SetPosition() // Generates an angle, then multiply that by a random magnitude (capping out at area size). Place the object at that point.
    {
        target = (Vector2)player.position + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(0f, zoneSize * 3);
    }
    void SetScale() // Increase the scale of the object to match the size. Alter the lifetime of the VFX to match the delay.
    {
        gameObject.transform.localScale = new Vector2(zoneSize, zoneSize);
    }
    void SetSubemitter()
    {
        sub.Play();
        var main = sub.main;
        main.startLifetime = zoneDelay;
        var emission = sub.emission;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.01f, 15 * zoneSize) });
        var shape = sub.shape;
        shape.radius = zoneSize;
    }
    void SetAOE()
    {
        par.Play();
        var main = par.main;
        main.startLifetime = zoneDelay;
        var emission = par.emission;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.01f, 8 * zoneSize) });
        var shape = par.shape;
        shape.radius = zoneSize;
    }
    void Update() // Enables the collider at once the delay ends, then disables the collider after 0.1f.
    {
        if (targetReached == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, travelSpeed * Time.deltaTime);
            
            if ((Vector2)transform.position == target) // Turns off the trail
            {
                targetReached = true;
                SetAOE();
                SetSubemitter();
                var emission = trail.emission;
                emission.rateOverTime = 0;
            }
        }
        else
        {
            zoneDelay -= Time.deltaTime;
            if (zoneDelay <= 0 && active == false)
            {
                colli.enabled = true;
                active = true;
            }
            if (zoneDelay <= -0.1f)
            {
                colli.enabled = false;
            }
            if (zoneDelay <= -1)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(damage); // Make sure to use currentDamage instead of enemyData.Damage in case of any damage multipliers in the future
            colli.enabled = false;
        }
    }
}
