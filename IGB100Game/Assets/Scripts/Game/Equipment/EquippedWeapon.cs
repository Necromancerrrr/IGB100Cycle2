using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// EquiuppedWeapon acts as the vessel for "shooting" or spawning objects.
// Attach this script to an empty object. Attach the object to the player.
// Can change the weapon type by dragging a Weapon object into the field in the inspector
public class EquippedWeapon : MonoBehaviour
{
    // Weapon "Bullet" Type
    public Weapon weaponType;

    // Save a reference of the player
    private Player player;

    // Direction in which the weapoon will "shoot" from
    Vector2 direction;

    // Weapon Shoot Timer
    public bool autoShoot = true;
    [SerializeField] private bool permanent = false;
    [SerializeField] private bool targetNearestEnemy = false;

    public float shootIntervalSeconds = 1.0f; // Change in inspector depending on the weapon
    public float shootDelaySeconds = 0.0f;
    public float shootTimer = 0.0f;
    float delayTimer = 0.0f;

    private void Awake()
    {
        // Get a reference of the player
        player = GetComponentInParent<Player>();

        UpdateWeaponCooldown();
    }

    private void Start()
    {
        if (permanent)
        {
            Shoot();
        }
    }

    private void Update()
    {
        ShootAtNearestEnemy();

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

    private void ShootAtNearestEnemy()
    {
        if (targetNearestEnemy && player.nearestEnemy != null)
        {
            float angle = Mathf.Atan2(player.nearestEnemy.transform.position.x, player.nearestEnemy.transform.position.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
            //transform.z.LookAt(player.nearestEnemy.transform.z);
        }
    }

    // "Fire" weapon; spawn the weapon projectile/hitbox
    private void Shoot()
    {
        GameObject go = Instantiate(weaponType.gameObject, transform.position, transform.rotation);
        Weapon goWeapon = go.GetComponent<Weapon>();
        goWeapon.weaponDirection = direction;
    }

    private void UpdateWeaponCooldown()
    {
        shootIntervalSeconds = shootIntervalSeconds / player.modCooldown;
    }
}