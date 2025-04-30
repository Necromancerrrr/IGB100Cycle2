using UnityEngine;

public class ProjectileCountPassive : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentProjectileCount = player.characterData.ProjectileCount * (1 + passiveItemData.Multiplier / 100f); // Works to make a percentage increase. Can make a flat number if you want.
    }
}
