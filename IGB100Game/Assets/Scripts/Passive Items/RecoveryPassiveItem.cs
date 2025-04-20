using UnityEngine;

public class RecoveryPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentRecovery *= 1 + passiveItemData.Multiplier / 100f; // Works to make a percentage increase
    }
}
