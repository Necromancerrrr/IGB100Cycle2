using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    // Movement
    [SerializeField] private float moveSpeed = 8.0f;
    [HideInInspector] public bool isFacingLeft;
    private Vector2 movement;
   
    // Find nearest enemy
    public GameObject nearestEnemy = null;
    

    // Experience and Levelling Up
    private int level = 0;
    private float experience = 0;
    [SerializeField] private List<float> experienceList;
    private float EXPTint = 1;

    // Player base stats
    public int playerHealth;

    // Stat levels
    private int DamageLevel = 0;
    private int FireRateLevel = 0;
    private int AOESizeLevel = 0;
    private int ProjectileCountLevel = 0;
    private int DurationLevel = 0;
    public float modProjectileSpeed = 10.0f;
    public float modCooldown = 2.0f;


    //Player Weapons
    public EquippedWeapon[] equippedWeapons;

    // Components
    private Rigidbody2D rb;
    /*
    // EquipmentList
    [SerializeField] private List<EquipmentAbstract> EquipmentList = new List<EquipmentAbstract>();
    [SerializeField] private List<float> EquipmentTimer = new List<float>();
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody attached to the player

        // Find base weapon on the player
        equippedWeapons = transform.GetComponentsInChildren<EquippedWeapon>();

    }

    // Update is called once per frame
    void Update()
    {
        TopDownMovement();
        // EquipmentCheck();
        EXPGainColour();
        FindNearestEnemy();
    }


    // Takes input from user and moves player -> Alter moveSpeed to adjust speed
    private void TopDownMovement()
    {
        movement.Set(InputManager.Movement.x, InputManager.Movement.y);

        rb.linearVelocity = movement * moveSpeed;

        // Save what direction the player was last facing
        if(movement.magnitude > 0.01f)
        {
            if (movement.x < 0)
            {
                isFacingLeft = true;
            }
            if (movement.x > 0)
            {
                isFacingLeft = false;
            }
        }
    }

    /*
    // Checks if equipment is ready and if so, activates
    private void EquipmentCheck()
    {
        for (int i = 0; i < EquipmentList.Count; i++)
        {
            EquipmentTimer[i] -= Time.deltaTime;
            if (EquipmentTimer[i] <= 0)
            {
                EquipmentTimer[i] = EquipmentList[i].WeaponCall(DamageLevel, FireRateLevel, AOESizeLevel, ProjectileCountLevel, DurationLevel);
            }
        }
    }
    */
    
    // Method which grants experience when coming into contact with Experience Orbs.
    // Also handles levelling up and calls for Equipment and Stat selection
    // Additionally, gaining EXP modifies EXPTint which affects player colour
    public void ExperienceGain(float EXP)
    {
        experience += EXP;
        EXPTint = Mathf.Clamp(EXPTint -= 0.1f, 0, 1);
        if (experience >= experienceList[level])
        {
            experience -= experienceList[level]; level++;
            // equipment & stat selection here
        }
    }

    // Sets colour tint based on EXPTint (seen in the method above). Also reduces EXPTint over time.
    private void EXPGainColour()
    {
        EXPTint = Mathf.Clamp(EXPTint += 0.5f * Time.deltaTime, 0, 1);
        GetComponent<SpriteRenderer>().color = new Color(EXPTint, EXPTint, 1, 1);
    }

    public void TakeDamage(int damageAmount)
    {
        playerHealth -= damageAmount;
    }

    
    // Checks for nearest enemy to player, weapons will call for var nearestEnemy
    private void FindNearestEnemy()
    {
        float distToNearestEnemy = Mathf.Infinity;
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach (GameObject currentEnemy in allEnemies)
        {
            
            float distToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            
            if (distToEnemy < distToNearestEnemy)
            {
                distToNearestEnemy = distToEnemy;
                nearestEnemy = currentEnemy;
            }
        }
    }
    
}
