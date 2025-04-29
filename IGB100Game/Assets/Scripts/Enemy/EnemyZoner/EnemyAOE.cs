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
        SetVFX();
    }
    void SetPosition() // Generates an angle, then multiply that by a random magnitude (capping out at area size). Place the object at that point.
    {
        target = (Vector2)player.position + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(0f, zoneSize);
    }
    void SetVFX() // Increase the scale of the object to match the size. Alter the lifetime of the VFX to match the delay.
    {
        par.Play();
        gameObject.transform.localScale = new Vector2(zoneSize, zoneSize);
        var main = par.main;
        main.startLifetime = zoneDelay;
        main.startSize = new ParticleSystem.MinMaxCurve(0.2f * zoneSize, 0.4f * zoneSize);
        var shape = par.shape;
        shape.radius = zoneSize;
    }
    void Update() // Enables the collider at once the delay ends, then disables the collider after 0.1f.
    {
        if (targetReached == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, travelSpeed * Time.deltaTime);
            par.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            if ((Vector2)transform.position == target)
            {
                targetReached = true;
                SetVFX();
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
            else if (zoneDelay <= -0.1f)
            {
                colli.enabled = false;
            }
            else if (zoneDelay <= -1)
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
