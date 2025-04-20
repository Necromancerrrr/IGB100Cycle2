using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats player;
    public PassiveItemScriptableObject passiveItemData;

    protected virtual void ApplyModifier()
    {
        // Apply modifier to the appropriate stat value in the child class
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerStats>();
        ApplyModifier();
    }
}
