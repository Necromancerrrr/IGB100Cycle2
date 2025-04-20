using UnityEngine;

public class ExperiencePickup : MonoBehaviour, ICollectable
{
    public int experienceAmount;
    public void Collect()
    {
        PlayerStats player = FindFirstObjectByType<PlayerStats>();
        player.IncreaseExperience(experienceAmount);
        Destroy(gameObject);
    }
}
