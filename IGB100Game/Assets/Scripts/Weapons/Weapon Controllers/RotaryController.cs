using UnityEngine;

public class RotaryController : WeaponController
{
    protected override void Attack()
    {
        base.Attack();
        float angle = Random.Range(0f, 360f);
        float clockwise = Random.Range(0, 2);
        GameObject rotaryInstance = Instantiate(weaponData.Prefab);
        rotaryInstance.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
        if (clockwise == 0) { rotaryInstance.GetComponent<RotaryBehaviour>().SetStats(angle, true); }
        else if (clockwise == 1) { rotaryInstance.GetComponent<RotaryBehaviour>().SetStats(angle, false); }
    }
}
