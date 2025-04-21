using UnityEngine;

public class ProjectileSpeedPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentProjectileSpeed *= 1 + passiveItemData.Multiplier / 100f; // Works to make a percentage increase
    }
}
