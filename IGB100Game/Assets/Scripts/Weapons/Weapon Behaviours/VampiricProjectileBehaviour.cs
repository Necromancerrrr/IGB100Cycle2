using UnityEngine;

public class VampiricProjectileBehaviour : MonoBehaviour
{
    // Stats
    float weaponDamage;
    float weaponSize;
    float weaponSpeed;

    // Visual Feedback Par
    [SerializeField] private Color parColour;
    [SerializeField] private GameObject par;
    private void Awake()
    {
        Destroy(gameObject, 5f);
    }
    public void SetStats(float damage, float size, float speed)
    {
        weaponDamage = damage;
        weaponSize = size;
        weaponSpeed = speed;
        SetScale();
    }
    void SetScale()
    {
        transform.localScale = new Vector2(weaponSize/20, weaponSize/20);
    }
    private void Update()
    {
        transform.position += transform.rotation * new Vector3(0, weaponSpeed, 0) * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, weaponSize / 4 + 0.5f); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
            GameObject parInstance = Instantiate(par);
            parInstance.GetComponent<HitParticle>().SetValues(transform.position, col.transform.position, parColour, 0.5f);
            Destroy(gameObject);
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(weaponDamage);
            }
            GameObject parInstance = Instantiate(par);
            parInstance.GetComponent<HitParticle>().SetValues(transform.position, col.transform.position, parColour, 0.5f);
            Destroy(gameObject);
        }
    }
}
