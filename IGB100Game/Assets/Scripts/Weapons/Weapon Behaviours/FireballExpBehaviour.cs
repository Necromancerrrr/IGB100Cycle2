using UnityEngine;

public class FireballExpBehaviour : MonoBehaviour
{
    float damage;
    float areaSize;
    float timer = 0.1f;

    [SerializeField] private ParticleSystem par;
    private CircleCollider2D colli;
    private void Awake()
    {
        colli = GetComponent<CircleCollider2D>();
        colli.enabled = true;
        Destroy(gameObject, 1);
    }
    public void WeaponStatsSet(WeaponScriptableObject stats)
    {
        damage = stats.Damage * FindFirstObjectByType<PlayerStats>().CurrentMight;
        areaSize = stats.AreaSize;
        var shape = par.shape;
        var emission = par.emission;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.01f, 50 * areaSize)});
        shape.radius = areaSize;
        colli.radius = areaSize;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && colli.enabled == true)
        {
            colli.enabled = false;
        }
    }
    protected void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(damage, transform.position, 2f);
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(damage);
            }
        }
    }
}
