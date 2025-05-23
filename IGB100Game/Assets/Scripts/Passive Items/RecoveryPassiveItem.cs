using UnityEngine;

public class RecoveryPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentRecovery = passiveItemData.Multiplier; // Recovery time is set to the item
    }
}
