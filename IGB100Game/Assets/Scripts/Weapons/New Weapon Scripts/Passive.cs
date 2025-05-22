using UnityEngine;

public class Passive : Item
{
    public PassiveData data;
    [SerializeField] CharacterData.Stats currentBoosts;

    [System.Serializable]
    public struct Modifier
    {
        public string name, description;
        public CharacterData.Stats boosts;
    }

    public virtual void Intialise(PassiveData data)
    {
        base.Intialise(data);
        this.data = data;
        currentBoosts = data.baseStats.boosts;
    }

    public virtual CharacterData.Stats GetBoosts()
    {
        return currentBoosts;
    }

    public override bool DoLevelUp()
    {
       base.DoLevelUp();

        if (!CanLevelUp())
        {
            return false;
        }

        currentBoosts += data.GetLevelData(++currentLevel).boosts;
        return true;
    }
}
