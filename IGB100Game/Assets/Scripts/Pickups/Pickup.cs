using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) // Destorys the object if it gets too close to the player
        {
            Destroy(gameObject);
        }
    }
}
