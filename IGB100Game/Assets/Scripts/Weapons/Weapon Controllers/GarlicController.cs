using UnityEngine;

public class GarlicController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedGarlic = Instantiate(weaponData.Prefab);
        spawnedGarlic.transform.position = transform.position; // Assign the position to be the same as this object which is parented to the player
        spawnedGarlic.transform.parent = transform; // So that it spawns below this object
    }
}
