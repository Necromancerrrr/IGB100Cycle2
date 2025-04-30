using UnityEngine;

public class MagnetPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentMagnet = player.characterData.Magnet * (1 + passiveItemData.Multiplier / 100f); // Works to make a percentage increase
    }
}