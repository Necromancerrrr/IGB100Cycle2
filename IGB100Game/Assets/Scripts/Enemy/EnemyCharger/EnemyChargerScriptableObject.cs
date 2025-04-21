using UnityEngine;

[CreateAssetMenu(fileName = "EnemyChargerScriptableObject", menuName ="ScriptableObjects/EnemyCharger")]
public class EnemyChargerScriptableObject : ScriptableObject
{
    [Header("Charger Stats")]
    [SerializeField]
    float chargeFrequency;
    public float ChargeFrequency { get => chargeFrequency; private set => chargeFrequency = value; }

    [SerializeField]
    float chargeSpeed;
    public float ChargeSpeed { get => chargeSpeed; private set => chargeSpeed = value; }

    [SerializeField]
    float chargeDuration;
    public float ChargeDuration { get => chargeDuration; private set => chargeDuration = value; }


    [SerializeField]
    float chargeFreeze;
    public float ChargeFreeze { get => chargeFreeze; private set => chargeFreeze = value; }
}
