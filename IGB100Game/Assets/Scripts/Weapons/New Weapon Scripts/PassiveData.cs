using UnityEngine;

[CreateAssetMenu(fileName = "Passive Data", menuName = "ScriptableObjects/NewPassive")]
public class PassiveData : ItemData
{
    public Passive.Modifier baseStats;
    public Passive.Modifier[] growth;

    public Passive.Modifier GetLevelData(int level)
    {
        if (level - 2 < growth.Length)
        {
            return growth[level - 2];
        }

        Debug.LogWarning(string.Format("Weapon doesn't have its level up stats for level {0}!", level));
        return new Passive.Modifier();
    }
}
