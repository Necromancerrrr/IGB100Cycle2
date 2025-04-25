using UnityEngine;

public class PactOfAdrenaline : PactItem
{
    protected override void ApplyPactModifier()
    {
        // Decrease Maximum Health
        player.CurrentHealth = 1;
        player.characterData.MaxHealth = 1;

        // Increase the players damage dramatically
        player.CurrentMight *= 4.0f;
    }
}
