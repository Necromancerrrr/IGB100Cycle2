using UnityEngine;

public class AreaSizePassive : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentAOE = player.characterData.AreaSize * (1 + passiveItemData.Multiplier / 100f); // Works to make a percentage increase. Can make a flat number if you want.
    }
}
