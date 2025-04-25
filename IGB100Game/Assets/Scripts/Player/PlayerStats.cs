using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.UI;
using TMPro;
using Unity.VisualScripting;
using JetBrains.Annotations;
using System;
using UnityEditor.Rendering;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData; // REMOVE PUBLIC BEFORE FINISHING

    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDeath;


    /// <summary>
    /// Player Stats ( DO NOT USE TO CHECK PLAYER STATS IN CODE )
    /// </summary>
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentProjectileDuration;
    float currentProjectileCount; // WIP
    float currentAOE; // WIP
    float currentMagnet;
    int currentKills;

    /// <summary>
    ///  Real Time Stat Tracking ( USE THESE FOR CHECKING PLAYER STATS IN CODE )
    /// </summary>
    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            // Check if the value has changed
            if (currentHealth != value)
            {
                //Update the real time value of the stat
                currentHealth = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth.ToString();
                }

            }
        }
    }
    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            // Check if the value has changed
            if (currentRecovery != value)
            {
                //Update the real time value of the stat
                currentRecovery = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery.ToString();
                }
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            // Check if the value has changed
            if (currentMight != value)
            {
                //Update the real time value of the stat
                currentMight = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentAttackDisplay.text = "Attack: " + currentMight.ToString();
                }
            }
        }
    }
    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            // Check if the value has changed
            if (currentMoveSpeed != value)
            {
                //Update the real time value of the stat
                currentMoveSpeed = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Movespeed: " + currentMoveSpeed.ToString();
                }
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            // Check if the value has changed
            if (currentProjectileSpeed != value)
            {
                //Update the real time value of the stat
                currentProjectileSpeed = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed.ToString();
                }
            }
        }
    }
    public float CurrentProjectileDuration
    {
        get { return currentProjectileDuration; }
        set
        {
            // Check if the value has changed
            if (currentProjectileDuration != value)
            {
                //Update the real time value of the stat
                currentProjectileDuration = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileDurationDisplay.text = "Projectile Duration: " + currentProjectileDuration.ToString();
                }

            }
        }
    }
    public float CurrentProjectileCount
    {
        get { return currentProjectileCount; }
        set
        {
            // Check if the value has changed
            if (currentProjectileCount != value)
            {
                //Update the real time value of the stat
                currentProjectileCount = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileCountDisplay.text = "Projectile Duration: " + currentProjectileCount.ToString();
                }

            }
        }
    }
    public float CurrentAOE
    {
        get { return currentAOE; }
        set
        {
            // Check if the value has changed
            if (currentAOE != value)
            {
                //Update the real time value of the stat
                currentAOE = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentAOEDisplay.text = "Projectile Duration: " + currentAOE.ToString();
                }
            }
        }
    }
    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            // Check if the value has changed
            if (currentMagnet != value)
            {
                //Update the real time value of the stat
                currentMagnet = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet.ToString();
                }
            }
        }
    }

    public int CurrentKills
    {
        get { return currentKills; }
        set
        {
            if (currentKills != value)
            {
                currentKills = value;
            }

            // Update player UI
            if (GameManager.instance != null)
            {
                GameManager.instance.currentKillsDisplay.text = "Kills: " + currentKills.ToString();
            }
        }
    }



    #endregion



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

    // Level Range List
    public List<LevelRange> levelRanges;

    // Inventory
    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;
    public int pactItemIndex;

    // Player Particles
    [Header("Player Particle Effects")]
    public ParticleSystem damageEffect;

    // Animation Script
    [Header("Player Animation")]
    PlayerAnimator playerAnimator;

    // Audio Feedback Script
    [Header("Player Audio")]
    PlayerAudio playerAudio;
 

    void Awake()
    {
        // Comment out these two lines if you want to play without going to character select scene
        
        if(characterData == null)
        {
            characterData = CharacterSelector.GetData();
            CharacterSelector.instance.DestorySingleton();
        }

        inventory = GetComponent<InventoryManager>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerAudio = GetComponent<PlayerAudio>();

        // Assign the variables
        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentProjectileDuration = characterData.ProjectileDuration;
        CurrentMagnet = characterData.Magnet;

        // Reset kills stat
        CurrentKills = 0;

        //Spawn the starting weapon
        SpawnWeapon(characterData.StartingWeapon);
    }

    void Start()
    {
        //Intialise the experience cap as the first experience cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;
        
        // Initialise Stat Tracking on Pause Menu
        GameManager.instance.currentHealthDisplay.text = "Health: \n" + currentHealth.ToString();
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: \n" + currentRecovery.ToString();
        GameManager.instance.currentAttackDisplay.text = "Attack: \n" + currentMight.ToString();
        GameManager.instance.currentMoveSpeedDisplay.text = "Movespeed: \n" + currentMoveSpeed.ToString();
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: \n" + currentProjectileSpeed.ToString();
        GameManager.instance.currentProjectileDurationDisplay.text = "Projectile Duration: \n" + currentProjectileDuration.ToString();
        GameManager.instance.currentMagnetDisplay.text = "Magnet: \n" + currentMagnet.ToString();


        GameManager.instance.currentKillsDisplay.text = "Kills: \n" + currentKills.ToString();
        GameManager.instance.AssignChosenCharacterUI(characterData);
        GameManager.instance.AssignLevelReachedUI(level);
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
        playerAudio.PlayEXPGainSound();
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

            // If the player is level 5, or level 10, make them choose a pact. Otherwise, make them choose a weapon/passive item
            switch (level) // Change to if else statement with % 5
            {
                case 5:
                    GameManager.instance.StartPactChoice();
                    break;

                case 10:
                    GameManager.instance.StartPactChoice();
                    break;

                default:
                    GameManager.instance.StartLevelUp();
                    break;
            }
        }
    }
    void PassiveHeal()
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime; // Heal per second

            if(Mathf.FloorToInt(CurrentRecovery * Time.deltaTime) % 2 == 0)
            {
                Debug.Log("Boom");
                OnPlayerDamaged.Invoke();
            }

            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth; // To ensure the player doesn't "overheal"
            }
        }
    }
    public void RestoreHealth(float amount)
    {
        // Only heal the player if their current health is less than their maximum
        if(CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;
            playerAudio.PlayHealthGainSound();

            OnPlayerDamaged?.Invoke();

            if (CurrentHealth > characterData.MaxHealth) 
            {
                CurrentHealth = characterData.MaxHealth; // To ensure the player doesn't "overheal"
            }
        }
        
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            CurrentHealth -= dmg;

            OnPlayerDamaged?.Invoke();

            if (damageEffect)
            {
                Instantiate(damageEffect, transform.position, Quaternion.identity);
            }

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                Kill();
            }
            else
            {
                playerAnimator.PlayPlayerHurtAnim();
                playerAudio.PlayPlayerHurtSound();
            }
        }
    }

    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            playerAnimator.PlayPlayerDeadAnim();
            playerAudio.PlayPlayerDeathSound();
            OnPlayerDeath?.Invoke();

            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponUISlots, inventory.passiveItemUISlots);
            GameManager.instance.GameOver();
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        //checking if inventory is full
        if(weaponIndex >= inventory.weaponSlots.Count)
        {
            Debug.LogError("Weapon Inventory slots already full");
            return;
        }

        //Spawn the starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);   // Set the weapon to be a child of the player
        inventory.AddWeapon(weaponIndex,spawnedWeapon.GetComponent<WeaponController>()); // Add the weapon to it's inventory slot

        weaponIndex++;
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        //checking if inventory is full
        if (passiveItemIndex >= inventory.passiveItemSlots.Count)
        {
            Debug.LogError("Passive Item Inventory slots already full");
            return;
        }

        //Spawn a passive item
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);   // Set the weapon to be a child of the player
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>()); // Add the weapon to it's inventory slot

        passiveItemIndex++;
    }

    public void SpawnPactItem(GameObject pactItem)
    {
        //checking if inventory is full
        if (pactItemIndex >= inventory.pactItemSlots.Count)
        {
            Debug.LogError("Pact Item Slots Are Full");
            return;
        }

        GameObject spawnedPactItem = Instantiate(pactItem, transform.position, Quaternion.identity);
        spawnedPactItem.transform.SetParent(transform);
        inventory.AddPactItem(pactItemIndex, spawnedPactItem.GetComponent<PactItem>());

        pactItemIndex++;

        
    }
}
