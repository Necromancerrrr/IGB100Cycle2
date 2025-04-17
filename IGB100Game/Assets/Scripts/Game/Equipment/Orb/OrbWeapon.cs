using UnityEngine;

public class OrbWeapon : Weapon
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetStats();
        
        // Check player facing direction and shoot from that direction
        if (player.isFacingLeft)
        {
            weaponDirection.x = -1;
        }
        else
        {
            weaponDirection.x = 1;
        }
        
        Destroy(gameObject, weaponLifeTime); // Destorys object after 'x' seconds
    }

    // Projectile code
    void Update()
    {
        weaponVelocity = weaponDirection * weaponProjectileSpeed;
    }

    private void FixedUpdate()
    {
        Vector2 pos = this.transform.position;

        pos += this.weaponVelocity * Time.fixedDeltaTime;

        this.transform.position = pos;
    }

    public void SetStats()
    {
        weaponDirection.x = 1; // Just shoots to the right, but this can be altered to any cardinal direction
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
