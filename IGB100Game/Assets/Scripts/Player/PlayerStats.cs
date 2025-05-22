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
    public CharacterData characterData; // REMOVE PUBLIC BEFORE FINISHING

    [SerializeField]
    CharacterData defaultCharacterData;
    public CharacterData.Stats baseStats;
    [SerializeField] CharacterData.Stats actualStats;

    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDeath;


    /// <summary>
    /// Player Stats ( DO NOT USE TO CHECK PLAYER STATS IN CODE )
    /// </summary>
    float health;
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
    public float MaxHealth
    {
        get { return actualStats.maxHealth; }
        set
        {
            // Check if the value has changed
            if (actualStats.maxHealth != value)
            {
                //Update the real time value of the stat
                actualStats.maxHealth = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = string.Format("Max Health: {0} / {1}", health, actualStats.maxHealth);
                }

            }
        }
    }
    public float CurrentHealth
    {
        get { return health; }
        set
        {
            // Check if the value has changed
            if (health != value)
            {
                //Update the real time value of the stat
                health = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = string.Format("Health: {0} / {1}", health, actualStats.maxHealth);
                }

            }
        }
    }
    public float CurrentRecovery
    {
        get { return actualStats.recovery; }
        set
        {
            // Check if the value has changed
            if (actualStats.recovery != value)
            {
                //Update the real time value of the stat
                actualStats.recovery = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + actualStats.recovery;
                }
            }
        }
    }

    public float CurrentMight
    {
        get { return actualStats.might; }
        set
        {
            // Check if the value has changed
            if (actualStats.might != value)
            {
                //Update the real time value of the stat
                actualStats.might = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentAttackDisplay.text = "Attack: " + actualStats.might;
                }
            }
        }
    }
    public float CurrentMoveSpeed
    {
        get { return actualStats.moveSpeed; }
        set
        {
            // Check if the value has changed
            if (actualStats.moveSpeed != value)
            {
                //Update the real time value of the stat
                actualStats.moveSpeed = value;

                // Update player UI
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Movespeed: " + actualStats.moveSpeed;
                }
            }
        }
    }


    public float CurrentProjectileSpeed
    {
        get { return Speed; }
        set { Speed = value; }
    }

    public float Speed
    {
        get { return actualStats.speed; }
        set
        {
            if(actualStats.speed != value)
            {
                actualStats.speed = value;

                if(GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + actualStats.speed;
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
        get { return Magnet; }
        set {  Magnet = value; }
    }

    public float Magnet
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
    PlayerInventory inventory;
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

        inventory = GetComponent<PlayerInventory>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerAudio = GetComponent<PlayerAudio>();
        impulse = GetComponent<CinemachineImpulseSource>();

        
        baseStats = actualStats = characterData.stats;
        health = actualStats.maxHealth;

        // Reset kills stat
        CurrentKills = 0;
    }

    void Start()
    {

        inventory.Add(characterData.StartingWeapon);

        //Intialise the experience cap as the first experience cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;

        // Initialise Stat Tracking on Pause Menu
        GameManager.instance.currentHealthDisplay.text = "Health: \n" + CurrentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: \n" + CurrentRecovery;
        GameManager.instance.currentAttackDisplay.text = "Attack: \n" + CurrentMight;
        GameManager.instance.currentMoveSpeedDisplay.text = "Movespeed: \n" + CurrentMoveSpeed;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: \n" + CurrentProjectileSpeed;
        GameManager.instance.currentProjectileDurationDisplay.text = "Projectile Duration: \n" + CurrentProjectileDuration;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: \n" + CurrentMagnet;
        GameManager.instance.currentKillsDisplay.text = "Kills: \n" + CurrentKills;

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
        _healthBarSlider.maxValue = MaxHealth;
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

    public void RecalculateStats()
    {
        actualStats = baseStats;
        foreach(PlayerInventory.Slot s in inventory.passiveSlots)
        {
            Passive p = s.item as Passive;
            if (p)
            {
                actualStats += p.GetBoosts();
            }
        }
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
        if (CurrentHealth != actualStats.maxHealth)
        {
            regenTimer -= Time.deltaTime;
            if (regenTimer < 0)
            {
                RestoreHealth(10);
                regenTimer = actualStats.recovery;
            }
        }
    }
    public void RestoreHealth(float amount)
    {
        // Only heal the player if their current health is less than their maximum
        Debug.Log("Attempting to heal" + amount);
        if(CurrentHealth < actualStats.maxHealth)
        {
            if (CurrentHealth + amount >= actualStats.maxHealth) { GameManager.GenerateFloatingText(actualStats.maxHealth - CurrentHealth, transform, healTextColour, 50); } // Displays abbreviated health amount if neccesary
            else { GameManager.GenerateFloatingText(amount, transform, healTextColour, 50); } // Displays heal text
            CurrentHealth += amount;
            playerAudio.PlayHealthGainSound();

            OnPlayerDamaged?.Invoke();

            if (CurrentHealth > actualStats.maxHealth) 
            {
                CurrentHealth = actualStats.maxHealth; // To ensure the player doesn't "overheal"
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
                Destroy(Instantiate(damageEffect, transform.position, Quaternion.identity), 5f);
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
}
