using UnityEngine;

public class OrbController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Attack()
    {
        base.Attack();
        GameObject orbInstance = Instantiate(weaponData.Prefab);
        orbInstance.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
        orbInstance.transform.parent = transform;
    }
}
