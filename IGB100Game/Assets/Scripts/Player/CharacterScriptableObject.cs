using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField]
    Sprite icon;
    public Sprite Icon { get => icon; set => icon = value; }

    [SerializeField]
    new string name;
    public string Name { get => name; set => name = value; }

    [SerializeField]
    GameObject startingWeapon;
    public GameObject StartingWeapon { get => startingWeapon; set => startingWeapon = value;}

    [SerializeField]
    float maxHealth;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    [SerializeField]
    float recovery;
    public float Recovery { get => recovery; set => recovery = value; }

    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    [SerializeField]
    float might;
    public float Might { get => might; set => might = value; }

    [SerializeField]
    float projectileSpeed;
    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }

    [SerializeField]
    float projectileCount;
    public float ProjectileCount { get => projectileSpeed; set => projectileCount = value; }

    [SerializeField]
    float projectileDuration;
    public float ProjectileDuration { get => projectileDuration; set => projectileDuration = value; }

    [SerializeField]
    float areaSize;
    public float AreaSize { get => areaSize; set => areaSize = value; }

    [SerializeField]
    float magnet;
    public float Magnet { get => magnet; set => magnet = value; }

    [SerializeField]
    float cooldown;
    public float Cooldown { get => cooldown; set => cooldown = value; }

    [SerializeField]
    float playerSize;
    public float PlayerSize { get => playerSize; set => playerSize = value; }

}
