using UnityEngine;

public class VampiricController : WeaponController
{
    protected override void Attack()
    {
        base.Attack();
        GameObject vampiricInstance = Instantiate(weaponData.Prefab);
        vampiricInstance.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
    }
}
