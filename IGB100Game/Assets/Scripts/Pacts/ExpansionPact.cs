using UnityEngine;

public class ExpansionPact : PactItem
{
    protected override void ApplyPactModifier()
    {
        // Make the player bigger
        player.transform.localScale *= 2.0f;

        // Reduce player move speed
        player.CurrentMoveSpeed *= 0.8f;

        // Increase the player's damage 
        player.CurrentMight *= 1.2f;
    }
}
