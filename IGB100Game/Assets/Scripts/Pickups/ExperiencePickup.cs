using UnityEngine;

public class ExperiencePickup : Pickup, ICollectable
{
    public int experienceAmount;
    public void Collect()
    {
        PlayerStats player = FindFirstObjectByType<PlayerStats>();
        player.IncreaseExperience(experienceAmount);
    }
}
