using UnityEngine;

public class AppleJuiceController : WeaponController
{
    public float maxSpawnRadius;
    Vector3 spawnTransform;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnAppleJuice = Instantiate(weaponData.Prefab);
        spawnTransform = new Vector3 (Random.Range(transform.position.x - maxSpawnRadius, transform.position.x + maxSpawnRadius), Random.Range(transform.position.y - maxSpawnRadius, transform.position.y + maxSpawnRadius), 0.0f);
        spawnAppleJuice.transform.position = spawnTransform;// Assign the position to be the same as this object, which is parented to the player
    }
}
