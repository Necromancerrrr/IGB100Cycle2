using UnityEngine;

public class DurationPassive : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentCDR = player.characterData.Cooldown * (1 + passiveItemData.Multiplier / 100f); // Works to make a percentage increase. Can make a flat number if you want.
    }
}
