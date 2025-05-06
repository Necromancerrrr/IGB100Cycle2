using UnityEngine;

public class RecoveryPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentRecovery = player.characterData.Recovery - passiveItemData.Multiplier; // Flat time reduction
    }
}
