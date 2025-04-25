using UnityEngine;

public class PurityPact : PactItem
{
    protected override void ApplyPactModifier()
    {
        // Increase Maximum Health
        player.characterData.MaxHealth += 10;
        
        // Heal the player to full health
        player.CurrentHealth = player.characterData.MaxHealth;
    }
}
