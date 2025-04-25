using UnityEngine;

public class FireballProjBehaviour : ProjectileWeaponBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject target;
    private Rigidbody2D rb;
    [SerializeField] private GameObject explosion;
    override protected void Start()
    {
        // Pulls stats and finds components
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        SetEnemy();
        Fire();
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
    private void Fire() // Fires towards target
    {
        Vector2 angle = rb.transform.position - target.transform.position;
        rb.linearVelocity -= angle.normalized * currentSpeed;
    }
    override protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        else if (col.CompareTag("Prop"))
        {
            Destroy(gameObject);
        }
    }
    protected void OnDestroy()
    {
        GameObject explosionInstance = Instantiate(explosion);
        explosionInstance.transform.position = transform.position;
        explosionInstance.GetComponent<FireballExpBehaviour>().WeaponStatsSet(weaponData);
    }
}
