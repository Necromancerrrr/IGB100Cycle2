using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollectArea;
    public float pullSpeed;

    void Start()
    {
        player = FindFirstObjectByType<PlayerStats>();
        playerCollectArea = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        playerCollectArea.radius = player.CurrentMagnet;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Check if the other game object has the ICollectable interface
        if(col.gameObject.TryGetComponent(out ICollectable collectible))
        {
            // Pulling animation
            // Get the rigidbody2D component on the item
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();

            // Vector2 pointing from the item to the player
            Vector2 forceDirection = (transform.position - col.transform.position).normalized;

            //Applies force to the item in the forceDirection
            rb.AddForce(forceDirection * pullSpeed);


            // If it does, call the collect method
            collectible.Collect();
        }
    }
}
