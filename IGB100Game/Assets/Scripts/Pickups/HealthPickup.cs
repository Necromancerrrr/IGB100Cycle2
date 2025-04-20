using UnityEngine;

public class HealthPickup : MonoBehaviour, ICollectable
{
    public int healAmount;
    public void Collect()
    {
        PlayerStats player = FindFirstObjectByType<PlayerStats>();
        player.HealDamage(healAmount);
        Destroy(gameObject);
    }
}
