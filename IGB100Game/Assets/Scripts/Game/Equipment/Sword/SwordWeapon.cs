using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SwordWeapon : Weapon
{
    void Start()
    {
        SetStats();

        // Check player facing direction
        if (player.isFacingLeft)
        {
            weaponDirection.x = -0.8f;
        }
        else
        {
            weaponDirection.x = 0.8f;
        }

        //Spawn in front of the player
        Vector2 pos = transform.position;
        pos.x += weaponDirection.x;
        transform.position = pos;

        Destroy(gameObject, weaponLifeTime); // Destorys object after 'x' seconds
    }

    public void SetStats()
    {
        weaponProjectileSpeed *= player.modProjectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAbstract enemy = collision.GetComponent<EnemyAbstract>();

        if (enemy != null)
        {
            IDamageable obj = (IDamageable)enemy;
            obj.TakeDamage(weaponDamage);
        }
    }
}
