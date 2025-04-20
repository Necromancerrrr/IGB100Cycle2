using UnityEngine;

/// <summary>
/// Base script of all Projectile behaviours [To be placed on a prefab of a weapon that is a projectile i.e. an Arrow]
/// </summary>

public class ProjectileWeaponBehaviour : MonoBehaviour
{

    protected Vector3 direction;
    public float destroyAfterSeconds;

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }


    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;
    }
}
