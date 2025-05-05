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
        for (int i = 0; i < weaponCount; i++)
        {
            GameObject spawnBoomerang = Instantiate(weaponData.Prefab);
            spawnBoomerang.transform.position = transform.position;
            if (i == 0)
            {
                spawnBoomerang.GetComponent<BoomerangBehaviour>().targetSet(false);
            }
            else { spawnBoomerang.GetComponent<BoomerangBehaviour>().targetSet(true); }
        }
         // Assign the position to be the same as this object, which is parented to the player
        //spawnBoomerang.GetComponent<BoomerangBehaviour>().DirectionChecker(pm.lastMovedVector); //Reference and set the direction
    }
}
