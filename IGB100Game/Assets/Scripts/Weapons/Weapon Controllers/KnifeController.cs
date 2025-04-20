using UnityEngine;

public class KnifeController : WeaponController
{



    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnKnife = Instantiate(weaponData.Prefab);
        spawnKnife.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
        spawnKnife.GetComponent<KnifeBehaviour>().DirectionChecker(pm.lastMovedVector); //Reference and set the direction
    }
}
