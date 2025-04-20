using UnityEngine;

public class MagnetPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentMagnet *= 1 + passiveItemData.Multiplier / 100f; // Works to make a percentage increase
    }
}