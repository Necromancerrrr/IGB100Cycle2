using System.Collections.Generic;
using UnityEngine;

public class BladestormBehaviour : MeleeWeaponBehaviour
{
    HashSet<GameObject> markedEnemies;
    public float hitResetInterval = 0.5f; // Time in seconds before enemies can be hit again
    private float timer;

    protected override void Start()
    {
        base.Start();
        markedEnemies = new HashSet<GameObject>();
        timer = hitResetInterval;
    }

    void Update()
    {
        // Clear marked enemies after the reset interval
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            markedEnemies.Clear();
            timer = hitResetInterval;
        }

        transform.RotateAround(transform.parent.position, Vector3.forward, 300 * Time.deltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && !markedEnemies.Contains(col.gameObject))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(GetCurrentDamage());
                markedEnemies.Add(col.gameObject);
            }
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.TryGetComponent(out BreakableProps breakable) && !markedEnemies.Contains(col.gameObject))
            {
                breakable.TakeDamage(GetCurrentDamage());
                markedEnemies.Add(col.gameObject);
            }
        }
    }
}
