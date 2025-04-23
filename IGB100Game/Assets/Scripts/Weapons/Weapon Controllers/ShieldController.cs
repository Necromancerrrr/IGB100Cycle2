using UnityEngine;

public class ShieldController : WeaponController
{
    override protected void Start()
    {
        base.Start();
    }
    protected override void Attack()
    {
        base.Attack();
        GameObject shieldInstance = Instantiate(weaponData.Prefab);
        shieldInstance.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
    }
}
