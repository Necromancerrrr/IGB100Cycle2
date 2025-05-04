using UnityEngine;

public class FlurryController : WeaponController
{
    protected override void Attack()
    {
        base.Attack();
        GameObject flurryInstance = Instantiate(weaponData.Prefab);
        flurryInstance.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
    }
}
