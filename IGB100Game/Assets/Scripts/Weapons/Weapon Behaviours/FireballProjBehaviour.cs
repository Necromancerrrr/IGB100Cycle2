using UnityEngine;

public class FireballProjBehaviour : ProjectileWeaponBehaviour
{
    private float angle;
    private Rigidbody2D rb;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject trail;

    [SerializeField] private AudioClip spawnAudio;

    override protected void Start()
    {
        // Pulls stats and finds components
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        SetEnemy();
        Fire();

        AudioManager.instance.PlaySFX(spawnAudio, transform, 1);
    }

    private void SetEnemy() // Selects the closest enemy as the target and grabs their angle. If there are no valid targets, self destruct
    {
        Vector2 target;
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
            Vector2 calc = target - (Vector2)player.transform.position;
            angle = 360 - (Mathf.Atan2(calc.x, calc.y) * Mathf.Rad2Deg);
        }
    }
    private void Update()
    {
        transform.position += transform.rotation * new Vector3(0, currentSpeed, 0) * Time.deltaTime;
    }
    private void Fire() // Fires towards target
    {
        transform.rotation = Quaternion.Euler(0, 0, angle + Random.Range(-30f, 30f));
    }
    override protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") || col.CompareTag("Prop"))
        {
            transform.DetachChildren();
            Destroy(sprite);
            var emission = trail.GetComponent<ParticleSystem>().emission;
            emission.rateOverTime = 0;
            Destroy(trail, 1);
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
