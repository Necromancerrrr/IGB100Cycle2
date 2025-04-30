using UnityEngine;

public class BootsPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentMoveSpeed = player.characterData.MoveSpeed * (1 + passiveItemData.Multiplier / 100f); // Works to make a percentage increase
    }
}
