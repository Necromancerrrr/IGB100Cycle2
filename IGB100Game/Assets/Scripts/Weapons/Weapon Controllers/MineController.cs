using UnityEngine;

public class MineController : WeaponController
{
    override protected void Start()
    {
        base.Start();
    }
    protected override void Attack()
    {
        base.Attack();
        GameObject mineInstance = Instantiate(weaponData.Prefab);
        mineInstance.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
    }
}
