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
        var shape = par.shape;
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
            enemy.TakeDamage(damage, transform.position, 2f); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
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
