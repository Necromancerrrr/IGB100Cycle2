using UnityEngine;

public class ContractionPact : PactItem
{
    protected override void ApplyPactModifier()
    {
        // Make the player smaller
        player.transform.localScale *= 0.5f;

        // Increase player move speed
        player.CurrentMoveSpeed *= 1.5f;

        // Decrease the player's damage 
        player.CurrentMight *= 0.8f;
    }
}
