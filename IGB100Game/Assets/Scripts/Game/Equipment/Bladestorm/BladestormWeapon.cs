using UnityEngine;

public class BladestormWeapon : Weapon
{
    void Start()
    {
        SetStats();

        // Check player facing direction and shoot from that directions
    }

    private void Update()
    {
        this.transform.position = player.transform.position;
    }

    private void FixedUpdate()
    {
        
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
