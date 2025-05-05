using System.Collections.Generic;
using UnityEngine;

public class BladestormBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> markedEnemies;
    public float hitResetInterval = 0.5f; // Time in seconds before enemies can be hit again
    private float timer;

    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
        timer = hitResetInterval;
        SetScale();
    }
    void SetScale()
    {
        transform.localScale = new Vector3(weaponSize, weaponSize, 1);
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
                enemy.TakeDamage(weaponDamage, transform.position, 5f);
                markedEnemies.Add(col.gameObject);
            }
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable) && !markedEnemies.Contains(col.gameObject))
            {
                breakable.TakeDamage(weaponDamage);
                markedEnemies.Add(col.gameObject);
            }
        }
    }
}