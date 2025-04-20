using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData; // REMOVE PUBLIC BEFORE FINISHING

    // Current stats
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentMagnet;

    // Spawned Weapon
    public List<GameObject> spawnedWeapons;

    //Experience and Level of the Player
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    // Class for defining a level range and the corresponding experience cap increase for that range
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    //I-Frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;
    void Awake()
    {
        /* <UNCOMMENT THIS OUT BEFORE BUILDING>
         
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestorySingleton();
       
        <UNCOMMENT THIS OUT BEFORE BUILDING>    */

        // Assign the variables
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;

        //Spawn the starting weapon
        SpawnWeapon(characterData.StartingWeapon);
    }

    void Start()
    {
        //Intialise the experience cap as the first experience cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;
    }
    void Update()
    {
        if (invincibilityTimer > 0) 
        { 
            invincibilityTimer -= Time.deltaTime;
        }

        //If the invulnerability timer has reached 0, make the player vulnerable again.
        else if (isInvincible)
        {
            isInvincible = false;
        }

        PassiveHeal();
    }
    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if(level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }

            experienceCap += experienceCapIncrease;
        }
    }
    void PassiveHeal()
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime; // Heal per second
            
            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth; // To ensure the player doesn't "overheal"
            }
        }
    }
    public void RestoreHealth(float amount)
    {
        // Only heal the player if their current health is less than their maximum
        if(currentHealth < characterData.MaxHealth)
        {
            currentHealth += amount;

            if (currentHealth > characterData.MaxHealth) 
            { 
                currentHealth = characterData.MaxHealth; // To ensure the player doesn't "overheal"
            }
        }
        
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            currentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (currentHealth <= 0)
            {
                Kill();
            }
        }

    }

    public void Kill()
    {
        Debug.Log("PLAYER IS DEAD");
    }

    public void SpawnWeapon(GameObject weapon)
    {
        //Spawn the starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);   // Set the weapon to be a child of the player
        spawnedWeapons.Add(spawnedWeapon);              // Add it to the list of spawned weapons
    }
}
