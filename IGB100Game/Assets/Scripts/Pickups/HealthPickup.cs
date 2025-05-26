using UnityEngine;

public class HealthPickup : Pickup, ICollectable
{
    public int healAmount;
    PlayerStats player;
    bool collecting = false;
    public void Collect()
    {
        player = FindFirstObjectByType<PlayerStats>();
        collecting = true;
    }
    public void SetValue(int value)
    {
        healAmount = value;
    }

    void FixedUpdate()
    {
        if (collecting)
        {
            var step = 10 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collecting && collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            player.RestoreHealth(healAmount);
        }
    }
}
