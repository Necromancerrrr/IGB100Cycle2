using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedWeapon : MonoBehaviour
{
    // Weapon "Bullet" Type
    public Weapon weaponType;

    private Player player;

    // Direction in which the weapoon will "shoot" from
    Vector2 direction;

    // Weapon Shoot Timer
    public bool autoShoot = true;

    public float shootIntervalSeconds = 1.0f; // Change in inspector depending on the weapon
    public float shootDelaySeconds = 0.0f;
    public float shootTimer = 0.0f;
    float delayTimer = 0.0f;

    private void Awake()
    {
        player = GetComponentInParent<Player>();

        UpdateWeaponCooldown();
    }

    private void Update()
    {
        // Automatically "shoot" the weapon every x seconds
        if (autoShoot) // Check to see if enabled
        {
            if (delayTimer >= shootDelaySeconds)
            {
                if (shootTimer >= shootIntervalSeconds)
                {
                    Shoot();
                    shootTimer = 0;
                }
                else
                {
                    shootTimer += Time.deltaTime;
                }
            }
            else
            {
                delayTimer += Time.deltaTime;
            }
        }
    }

    // "Fire" weapon; spawn the weapon projectile/hitbox
    private void Shoot()
    {
        GameObject go = Instantiate(weaponType.gameObject, transform.position, Quaternion.identity);
        Weapon goWeapon = go.GetComponent<Weapon>();
        goWeapon.weaponDirection = direction;
    }

    private void UpdateWeaponCooldown()
    {
        shootIntervalSeconds = shootIntervalSeconds / player.modCooldown;
    }
}



