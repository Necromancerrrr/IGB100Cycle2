using UnityEngine;

public class VampiricProjectileBehaviour : MonoBehaviour
{
    float weaponDamage;
    float weaponSize;
    private void Awake()
    {
        Destroy(gameObject, 5f);
    }
    public void SetStats(float damage, float size, float angle, float speed)
    {
        weaponDamage = damage;
        weaponSize = size;
        SetScale();
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Mathf.Cos(angle) * Mathf.Rad2Deg, Mathf.Sin(angle) * Mathf.Rad2Deg).normalized * speed;
    }
    void SetScale()
    {
        transform.localScale = new Vector2(weaponSize/10, weaponSize/10);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, weaponSize / 4 + 1); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
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
