using UnityEngine;

public class OrbWeapon : Weapon
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        SetStats();
    }

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
        weaponDirection.x = 1;
        weaponProjectileSpeed *= player.modProjectileSpeed;
    }
}
