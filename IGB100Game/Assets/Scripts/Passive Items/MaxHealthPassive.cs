using UnityEngine;

public class MaxHealthPassive : PassiveItem
{
    protected override void ApplyModifier()
    {
        float currentDamage = player.CurrentMaxHealth - player.CurrentHealth;
        Debug.Log("Reducing" + currentDamage);
        player.CurrentMaxHealth = player.characterData.MaxHealth + passiveItemData.Multiplier; // Flat health increase
        player.CurrentHealth = player.CurrentMaxHealth - currentDamage;
    }
}
