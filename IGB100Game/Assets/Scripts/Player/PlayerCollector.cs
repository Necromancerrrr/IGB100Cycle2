using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Check if the other game object has the ICollectable interface
        if(col.gameObject.TryGetComponent(out ICollectable collectible))
        {
            // If it does, call the collect method
            collectible.Collect();
        }
    }
}
