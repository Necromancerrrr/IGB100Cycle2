using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
//using Unity.UI;
using TMPro;
using Unity.VisualScripting;
using JetBrains.Annotations;
using System;
using Unity.Cinemachine;
//using UnityEditor.Rendering;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData; // REMOVE PUBLIC BEFORE FINISHING

    [SerializeField]
    CharacterScriptableObject defaultCharacterData;

    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDeath;


    /// <summary>
    /// Player Stats ( DO NOT USE TO CHECK PLAYER STATS IN CODE )
    /// </summary>
    float currentHealth;
    float currentMaxHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentProjectileDuration;
    float currentProjectileCount;
    float currentAOE;
    float currentCDR;
    float currentMagnet;
    int currentKills;

    /// <summary>
    ///  Real Time Stat Tracking ( USE THESE FOR CHECKING PLAYER STATS IN CODE )
    /// </summary>
    #region Current Stats Properties
    public float CurrentMaxHealth
    {
        get { return currentMaxHealth; }
        set
        {
            // Check if the value has changed
            if (currentMaxHealth != value)
            {
                //Update the real time value of the stat
                currentMaxHealth = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Max Health: " + currentHealth.ToString();
                }

            }
        }
    }
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
                    GameManager.instance.currentProjectileDurationDisplay.text = "Duration: " + currentProjectileDuration.ToString();
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
                    GameManager.instance.currentProjectileCountDisplay.text = "Projectile Count: " + currentProjectileCount.ToString();
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
                    GameManager.instance.currentAOEDisplay.text = "AOE: " + currentAOE.ToString();
                }
            }
        }
    }
    public float CurrentCDR
    {
        get { return currentCDR; }
        set
        {
            // Check if the value has changed
            if (currentCDR != value)
            {
                //Update the real time value of the stat
                currentCDR = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    //GameManager.instance.currentCDRDisplay.text = "Cooldown reduction: " + currentAOE.ToString();
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


    // Experience and Level of the Player
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;
    public XPUIManager XPBar;

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

    //Player UI
    [Header("Player UI")]
    [SerializeField] 
    public Slider _healthBarSlider;

    // Player Particles
    [Header("Player Particle Effects")]
    public ParticleSystem damageEffect;

    // Animation Script
    [Header("Player Animation")]
    PlayerAnimator playerAnimator;

    // Audio Feedback Script
    [Header("Player Audio")]
    PlayerAudio playerAudio;

    // Player damaged impulse
    CinemachineImpulseSource impulse;

    // Damaged & Heal text
    Color32 healTextColour = Color.green;
    Color32 damagedTextColour = Color.red;
    void Awake()
    {
        
        if(CharacterSelector.instance != null)
        {
            characterData = CharacterSelector.GetData();
        }
        else
        {
                characterData = defaultCharacterData;
        }

        inventory = GetComponent<InventoryManager>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerAudio = GetComponent<PlayerAudio>();
        impulse = GetComponent<CinemachineImpulseSource>();

        // Assign the variables
        CurrentMaxHealth = characterData.MaxHealth;
        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentProjectileDuration = characterData.ProjectileDuration;
        CurrentMagnet = characterData.Magnet;
        currentProjectileCount = characterData.ProjectileCount;
        currentAOE = characterData.AreaSize;
        currentCDR = characterData.Cooldown;

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

        // Set health bar
        SetHealthBar();

        // Set XP Bar
        XPBar.SetXPCap(experienceCap);
        XPBar.SetXPBar(experience);
    }
    public void SetHealthBar()
    {
        _healthBarSlider.maxValue = CurrentMaxHealth;
        _healthBarSlider.value = CurrentHealth;
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
            movespeedMod = 1;
        }
        if (isFrozen == true && TimerIsDone() == true && GameObject.FindWithTag("GameController").GetComponent<GameManager>().currentState != GameManager.GameState.Pause)
        {
            Time.timeScale = originalTimeScale;
            isFrozen = false;
        }
        _healthBarSlider.value = CurrentHealth;
        PassiveHeal();


        // Developer Shortcuts



    }
    public void IncreaseExperience(int amount)
    {
        experience += amount;
        XPBar.SetXPBar(experience);
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
            XPBar.SetXPCap(experienceCap);
            XPBar.SetXPBar(experience);

            GameManager.instance.StartLevelUp();
            // If the player is level 5, or level 10, make them choose a pact. Otherwise, make them choose a weapon/passive item
            //switch (level) // Change to if else statement with % 5
            //{
            //   case 5:
            //        GameManager.instance.StartPactChoice();
            //        break;
            //
            //    case 10:
            //        GameManager.instance.StartPactChoice();
            //        break;
            //
            //    default:
            //        GameManager.instance.StartLevelUp();
            //        break;
            //}
        }
    }
    // Variables for regen
    private float regenTimer = 1;
    void PassiveHeal()
    {
        if (CurrentRecovery != characterData.Recovery)
        {
            
            regenTimer -= Time.deltaTime;
            if (regenTimer < 0)
            {
                RestoreHealth(10);
                regenTimer = CurrentRecovery;
            }
        }
    }
    public void RestoreHealth(float amount)
    {
        // Only heal the player if their current health is less than their maximum
        Debug.Log("Attempting to heal" + amount);
        if(CurrentHealth < CurrentMaxHealth)
        {
            if (CurrentHealth + amount >= CurrentMaxHealth) { GameManager.GenerateFloatingText(CurrentMaxHealth - CurrentHealth, transform, healTextColour, 50); } // Displays abbreviated health amount if neccesary
            else { GameManager.GenerateFloatingText(amount, transform, healTextColour, 50); } // Displays heal text
            CurrentHealth += amount;
            playerAudio.PlayHealthGainSound();

            OnPlayerDamaged?.Invoke();

            if (CurrentHealth > CurrentMaxHealth) 
            {
                CurrentHealth = CurrentMaxHealth; // To ensure the player doesn't "overheal"
            }
        }
        
    }
    public float movespeedMod = 1;
    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            CurrentHealth -= dmg;
            

            impulse.GenerateImpulse();
            GameManager.GenerateFloatingText(dmg, transform, damagedTextColour, 50);
            //Freeze(0.5f);

            OnPlayerDamaged?.Invoke();

            if (damageEffect)
            {
                Instantiate(damageEffect, transform.position, Quaternion.identity);
            }

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;
            movespeedMod = 1.3f;

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
    // Freeze variables
    bool isFrozen;
    float originalTimeScale;
    float duration;
    float lastTime;
    private void Freeze(float duration)
    {
        if (!isFrozen) // isFrozen tracks when the game should be in freeze frame
        {
            originalTimeScale = Time.timeScale; // Record timeScale before freezing
            isFrozen = true;
            Time.timeScale = 0.1f; // Slow down timescale
            StartTimer(duration);  // Start the timer
        }
    }
    public void StartTimer(float newDuration)
    {
        duration = newDuration;
        lastTime = Time.unscaledTime; // Record the start time (unscaledTime records seconds since game start)
    } // Call to start time scale independent timer
    public bool TimerIsDone()
    {
        return (Time.unscaledTime - lastTime) >= duration;
        // return current time stamp - lastTime(timestamp since freezeframe start) = duration elapsed
    } // Call to check if timer is done
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
   
}
