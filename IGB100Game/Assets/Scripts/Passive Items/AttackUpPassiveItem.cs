using UnityEngine;

public class AttackUpPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentMight = player.characterData.Might * (1 + passiveItemData.Multiplier / 100f); // Works to make a percentage increase. Can make a flat number if you want.
    }
}
