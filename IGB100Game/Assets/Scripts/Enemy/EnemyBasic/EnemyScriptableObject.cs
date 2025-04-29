using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName ="ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    // Base stats for enemies
    [Header("Enemy Base Stats")]
    [SerializeField]
    public float maxHealth;
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }

    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }

    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }
    [SerializeField]
    float knockbackMod;
    public float KnockbackMod { get => knockbackMod; private set => knockbackMod = value; }
}
