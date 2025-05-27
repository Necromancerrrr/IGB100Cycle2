using UnityEngine;
using UnityEngine.UIElements;

public class RotaryProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject sprite;
    float weaponDamage;
    float weaponSpeed;
    float rot;
    bool clockwise;

    // Visual Feedback Par
    [SerializeField] private Color parColour;
    [SerializeField] private GameObject par;
    private void Awake()
    {
        Destroy(gameObject, 5f);
    }
    public void SetStats(float damage, float speed)
    {
        weaponDamage = damage;
        weaponSpeed = speed;
        rot = Random.Range(0f, 360f);
        if (Random.Range(0, 2) == 0 ) { clockwise = true; }
        else { clockwise = false; }
    }
    private void Update() // Moves in the direction it is currently facing
    {
        transform.position += transform.rotation * new Vector3(0, weaponSpeed, 0) * Time.deltaTime;
        if (clockwise == true) { rot += 20 * Time.deltaTime; }
        else { rot -= 20 * Time.deltaTime; }
        sprite.transform.rotation = Quaternion.Euler(0, 0, rot);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, 0.5f); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
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
