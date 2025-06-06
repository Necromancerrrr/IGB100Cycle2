using UnityEngine;

public class HealthPickup : Pickup, ICollectable
{
    public int healAmount;
    PlayerStats player;
    public bool collecting = false;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
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
            //var step = 10 * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);

            // Above this is the old code, below is the new one

            Vector2 angle = rb.transform.position - player.transform.position;
            rb.linearVelocity -= angle.normalized * 20 * Time.deltaTime;
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
