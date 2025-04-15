using System.Collections.Generic;
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

    // Components
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody attached to the player
    }

    // Update is called once per frame
    void Update()
    {
        TopDownMovement();
    }


    // Takes input from user and moves player -> Alter moveSpeed to adjust speed
    private void TopDownMovement()
    {
        movement.Set(InputManager.Movement.x, InputManager.Movement.y);

        rb.linearVelocity = movement * moveSpeed;
    }
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
