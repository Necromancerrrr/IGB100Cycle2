using UnityEngine;

[CreateAssetMenu(fileName ="PassiveItemScriptableObject", menuName ="ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multiplier;
    public float Multiplier { get => multiplier; set => multiplier = value; }

    [SerializeField]
    int level;      // NOT meant to be modified in the game [Only in Editor]
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab; // The prefab of the next level i.e. what rhe object becomes when it levels up
                                // Not to be confused with the prefab to be spawned at the next level
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField] // Passive item name
    new string name;
    public string Name { get => name; set => name = value; }

    [SerializeField] // Description of the passive item (what it does)
    string description;
    public string Description { get => description; set => description = value; }

    [SerializeField]
    Sprite icon;  // Not meant to be modified in game [only in Editor]
    public Sprite Icon { get => icon; private set => icon = value; }
}
