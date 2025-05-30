using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GarlicBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> markedEnemies;
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy") && !markedEnemies.Contains(col.gameObject))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, 3);
            markedEnemies.Add(col.gameObject); // Mark the enemy so it doesn't take another instance of damage from this "garlic"
        }
        else if (col.CompareTag("Prop") )
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable) && !markedEnemies.Contains(col.gameObject))
            {
                breakable.TakeDamage(weaponDamage);

                markedEnemies.Add(col.gameObject);
            }
        }
    }
}
