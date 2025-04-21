using UnityEngine;

[CreateAssetMenu(fileName = "EnemyShooterScriptableObject", menuName ="ScriptableObjects/EnemyShooter")]
public class EnemyShooterScriptableObject : ScriptableObject
{
    [Header("Shooter Stats")]
    [SerializeField]
    float firingFrequency;
    public float FiringFrequency { get => firingFrequency; private set => firingFrequency = value; }

    [SerializeField]
    float projectileCount;
    public float ProjectileCount { get => projectileCount; private set => projectileCount = value; }

    [SerializeField]
    float projectileInterval;
    public float ProjectileInterval { get => projectileInterval; private set => projectileInterval = value; }

    [SerializeField]
    float projectileSpeed;
    public float ProjectileSpeed { get => projectileSpeed; private set => projectileSpeed = value; }
}
