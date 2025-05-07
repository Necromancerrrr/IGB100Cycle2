using UnityEngine;

public class FlurryProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject spriteRenderer;
    bool clockwise;
    float angle;
    float weaponDamage;
    float weaponSpeed;
    private void Awake()
    {
        Destroy(gameObject, 5f);
    }
    public void SetStats(float damage, float speed)
    {
        weaponDamage = damage;
        weaponSpeed = speed;
        angle = Random.Range(0f, 360f);
        if (Random.Range(0, 2) == 0) { clockwise = true; }
        else { clockwise = false; }
    }
    private void Update() // Moves in the direction it is currently facing
    {
        Spin();
        Movement();
    }
    void Spin()
    {
        if (clockwise == true) { angle += 200 * Time.deltaTime; }
        else { angle -= 200 * Time.deltaTime; }
        spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void Movement()
    {
        if (weaponSpeed >= 0) { weaponSpeed -= weaponSpeed * Time.deltaTime;  }
        transform.position += transform.rotation * new Vector3(0, weaponSpeed, 0) * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, 1); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
            Destroy(gameObject);
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(weaponDamage);
            }
            Destroy(gameObject);
        }
    }
}
