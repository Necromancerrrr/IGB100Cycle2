using UnityEngine;

public class MineBehaviour : ProjectileWeaponBehaviour
{
    [SerializeField] private GameObject damagePrefab;
    new void Start()
    {
        base.Start();
        transform.localScale = new Vector3(1 + weaponSize / 10, 1 + weaponSize / 10, 1);
    }

    // Update is called once per frame
    protected new void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            GameObject damageInstance = Instantiate(damagePrefab);
            damageInstance.GetComponent<MineDamageBehaviour>().SetStats(transform.position, weaponDamage, weaponSize);
            Destroy(gameObject);
        }
    }
}
