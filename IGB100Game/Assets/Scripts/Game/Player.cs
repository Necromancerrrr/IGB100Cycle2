using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Movement
    [SerializeField] private float moveSpeed = 8.0f;
    private Vector2 movement;

    // Experience and Levelling Up
    private int level = 0;
    private float experience = 0;
    [SerializeField] private List<float> experienceList;

    // Stat levels
    private int DamageLevel = 0;
    private int FireRateLevel = 0;
    private int AOESizeLevel = 0;
    private int ProjectileCountLevel = 0;
    private int DurationLevel = 0;

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
    }

    // Update is called once per frame
    void Update()
    {
        TopDownMovement();
        // EquipmentCheck();
    }


    // Takes input from user and moves player -> Alter moveSpeed to adjust speed
    private void TopDownMovement()
    {
        movement.Set(InputManager.Movement.x, InputManager.Movement.y);

        rb.linearVelocity = movement * moveSpeed;
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
    
    public void ExperienceGain(float EXP)
    {
        experience += EXP;
        // some sort of visual feedback???
        if (experience >= experienceList[level])
        {
            experience -= experienceList[level]; level++;
            // equipment stat selection here
        }
    }
    
}
