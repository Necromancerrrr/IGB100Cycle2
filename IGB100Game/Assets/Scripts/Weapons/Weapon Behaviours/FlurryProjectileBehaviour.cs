using UnityEngine;

public class FlurryProjectileBehaviour : MonoBehaviour
{
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
    }
    private void Update() // Moves in the direction it is currently facing
    {
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
