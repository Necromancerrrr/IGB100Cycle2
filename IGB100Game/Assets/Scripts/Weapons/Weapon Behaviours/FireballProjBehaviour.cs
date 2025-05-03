using UnityEngine;

public class FireballProjBehaviour : ProjectileWeaponBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector2 target;
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
            target = enemies[0].transform.position;
            GameObject player = GameObject.FindWithTag("Player");
            foreach (GameObject enemy in enemies)
            {
                if ((enemy.transform.position - player.transform.position).magnitude <= (target - (Vector2)player.transform.position).magnitude)
                {
                    target = enemy.transform.position;
                }
            }
            float magMod = ((Vector2)player.transform.position - target).magnitude;
            target += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(0f, 0.5f * magMod);
        }
    }
    private void Fire() // Fires towards target
    {
        Vector2 angle = (Vector2)rb.transform.position - target;
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
    protected void OnDestroy() // Upon self destructing, create an explosion instance at current location with appropriate damage and size
    {
        GameObject explosionInstance = Instantiate(explosion);
        explosionInstance.transform.position = transform.position;
        explosionInstance.GetComponent<FireballExpBehaviour>().WeaponStatsSet(weaponDamage, weaponSize);
    }
}
