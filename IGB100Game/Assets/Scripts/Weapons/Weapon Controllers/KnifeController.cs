using UnityEngine;

public class KnifeController : WeaponController
{
    float timer;
    float ammo;
    float interval;
    Vector3 dir;
    Vector3 dir2;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        timer = 0;
        ammo = weaponCount;
        interval = 0.3f / (ammo - 1);
        dir = pm.delayDir;
        dir2 = Vector3.zero - (Vector3)pm.delayDir;
    }
    new private void Update()
    {
        base.Update();
        timer -= Time.deltaTime;
        if (timer <= 0 && ammo > 0)
        {
            Fire();
            timer = interval;
        }
    }
    private void Fire()
    {
        ammo -= 1;
        GameObject spawnKnife = Instantiate(weaponData.Prefab);
        spawnKnife.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
        spawnKnife.GetComponent<KnifeBehaviour>().DirectionChecker(dir); //Reference and set the direction
        GameObject spawnKnifeBack = Instantiate(weaponData.Prefab);
        spawnKnifeBack.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
        spawnKnifeBack.GetComponent<KnifeBehaviour>().DirectionChecker(dir2);
    }
}
