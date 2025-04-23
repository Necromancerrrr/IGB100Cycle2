using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField]
    GameObject prefab;

    public GameObject Prefab { get => prefab; private set => prefab = value; }
    // Base stats for weapons
    [Header("Weapon Stats")]
    [SerializeField] // Damage of the weapon
    float damage;
    public float Damage { get => damage; private set => damage = value; }

    [SerializeField] // Speed of the weapon (effect varies)
    float speed;
    public float Speed { get => speed; private set => speed = value; }

    [SerializeField] // How often the weapon activates
    float cooldownDuration;
    public float CooldownDuration { get => cooldownDuration; private set => cooldownDuration = value; }

    [SerializeField]
    int pierce; // How many enemies the projectile can collide with before self-terminating
    public int Pierce { get => pierce; private set => pierce = value; }

    [SerializeField]
    float areaSize; // How large the size of the collider of a weapon becomes
    public float AreaSize { get => areaSize; private set => areaSize = value; }

    [SerializeField]
    float projectileCount; // How many projectiles are created per activation
    public float ProjectileCount { get => projectileCount; private set => projectileCount = value; }

    [SerializeField]
    float duration; // How long effects of the weapon last (varies)
    public float Duration { get => duration; private set => duration = value; }

    [SerializeField]
    int level;      // NOT meant to be modified in the game [Only in Editor]
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab; // The prefab of the next level i.e. what rhe object becomes when it levels up
                                // Not to be confused with the prefab to be spawned at the next level
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField] // Weapon name
    new string name;
    public string Name { get => name; set => name = value; }

    [SerializeField] // Description of the weapon (what it does)
    string description;
    public string Description { get => description; set => description = value; }

    [SerializeField]
    Sprite icon;  // Not meant to be modified in game [only in Editor]
    public Sprite Icon { get => icon; private set => icon = value; }

}
