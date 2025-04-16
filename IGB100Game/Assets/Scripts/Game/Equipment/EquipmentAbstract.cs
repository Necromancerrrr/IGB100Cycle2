using UnityEngine;

public abstract class EquipmentAbstract : MonoBehaviour
{
    protected int level = 1;
    protected GameObject player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public abstract float WeaponCall(float DamageLevel, float FireRateLevel, float AOESizeLevel, float ProjectileCountLevel, float DurationLevel);
}
