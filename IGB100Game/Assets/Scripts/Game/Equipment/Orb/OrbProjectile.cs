using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class OrbProjectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem OrbAura;
    [SerializeField] private float TickFrequency;
    [SerializeField] private float TickDuration;
    [SerializeField] private float ParticleBurst;
    private CircleCollider2D Col;  
    private float Damage;
    private float AOESize;
    private float Duration;
    private float Timer = 0;
    private float BurstTimer = 0.1f;
    private bool ObjectActive = true;
    private bool HitboxActive = false;

    private void Awake()
    {
        Col = GetComponent<CircleCollider2D>();
        Col.enabled = false;
    }
    public void OnInstantiate(float Dam, float AOE, float Dur)
    {
        Damage = Dam; AOESize = AOE; Duration = Dur;
        Col.radius = AOESize;
        var parti = OrbAura.shape;
        parti.radius = AOESize;
    }
    
    void Update()
    {
        DurationCheck();
        if (ObjectActive == true) { DamageCheck(); }
    }
    // Disables object once duration ends (but allows enough time for particles to fade)
    void DurationCheck() 
    {
        Duration -= Time.deltaTime;
        if (Duration <= 0 && ObjectActive == true)
        {
            ObjectActive = false;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<SpriteRenderer>().enabled = false;
            var parti = OrbAura.emission;
            parti.rateOverTime = 0;
        }
        if (Duration <= -2)
        {
            Destroy(gameObject);
        }
    }
    // Enables trigger for damage, then disables it after a delay
    void DamageCheck()
    {
        Timer -= Time.deltaTime;
        BurstTimer += Time.deltaTime;
        if (Timer <= 0)
        {
            HitboxActive = true;
            Col.enabled = true;
            Timer = TickFrequency;
            var parti = OrbAura.emission;
            parti.SetBursts(new ParticleSystem.Burst[] {new ParticleSystem.Burst(BurstTimer, ParticleBurst)});
        }
        else if (Timer <= TickFrequency - TickDuration && HitboxActive == true)
        {
            HitboxActive = false;
            Col.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyAbstract>().TakeDamage(Damage);
        }
    }
}
