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
            weaponDirection.x = -1;
        }
        else
        {
            weaponDirection.x = 1;
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
}
