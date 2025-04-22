using UnityEngine;

public class PactItem : MonoBehaviour
{
    protected PlayerStats player;
    public PactItemScriptableObject pactItemData; // Not used for stats as of right now.

    protected virtual void ApplyPactModifier()
    {
        // Apply modifier to the appropriate stat value in the child class
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerStats>();
        ApplyPactModifier();
    }
}
