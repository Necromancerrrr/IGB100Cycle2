using UnityEngine;

public class HealthPickup : Pickup, ICollectable
{
    public int healAmount;
    public void Collect()
    {
        PlayerStats player = FindFirstObjectByType<PlayerStats>();
        player.RestoreHealth(healAmount);
    }
}
