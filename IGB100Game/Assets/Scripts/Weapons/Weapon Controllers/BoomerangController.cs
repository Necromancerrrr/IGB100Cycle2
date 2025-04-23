using UnityEngine;

public class BoomerangController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnBoomerang = Instantiate(weaponData.Prefab);
        spawnBoomerang.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
        spawnBoomerang.GetComponent<BoomerangBehaviour>().DirectionChecker(pm.lastMovedVector); //Reference and set the direction
    }
}
