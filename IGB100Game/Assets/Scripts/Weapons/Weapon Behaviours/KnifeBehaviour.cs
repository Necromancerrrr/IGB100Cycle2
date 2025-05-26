using UnityEngine;

public class KnifeBehaviour : ProjectileWeaponBehaviour
{
    // HitFX values
    [SerializeField] private Color parColour;
    [SerializeField] private GameObject par;

    

    protected override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * currentSpeed * Time.deltaTime; // Set the movement of the knife
    }
    protected new void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, 1.5f); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
            GameObject parInstance = Instantiate(par);
            parInstance.GetComponent<HitParticle>().SetValues(transform.position, col.transform.position, parColour, 0.5f);
            ReducePierce();
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(weaponDamage);
            }
            GameObject parInstance = Instantiate(par);
            parInstance.GetComponent<HitParticle>().SetValues(transform.position, col.transform.position, parColour, 0.5f);
            ReducePierce();
        }
    }
}
