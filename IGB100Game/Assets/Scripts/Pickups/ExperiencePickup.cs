using Unity.VisualScripting;
using UnityEngine;

public class ExperiencePickup : Pickup, ICollectable
{
    public int experienceAmount;
    PlayerStats player;
    bool collecting = false;
    public void Collect()
    {
        player = FindFirstObjectByType<PlayerStats>();
        collecting = true;
    }
    public void SetValue(int value)
    {
        experienceAmount = value;
    }
    
    void FixedUpdate()
    {
        if (collecting) 
        {
            var step = 10 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collecting && collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            player.IncreaseExperience(experienceAmount);
        }
    }
  
}
