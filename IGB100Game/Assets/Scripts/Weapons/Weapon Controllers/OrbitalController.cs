using UnityEngine;

public class OrbitalController : WeaponController
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
        GameObject orbitalInstance = Instantiate(weaponData.Prefab);
        orbitalInstance.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
        orbitalInstance.transform.parent = transform;
    }
}
