using UnityEngine;

[CreateAssetMenu(fileName = "PactItemScriptableObject", menuName = "ScriptableObjects/Pact")]
public class PactItemScriptableObject : ScriptableObject
{
    [SerializeField] // Pact name
    new string name;
    public string Name { get => name; set => name = value; }

    [SerializeField] // Description of the Pact (what it does)
    string description;
    public string Description { get => description; set => description = value; }

    [SerializeField]
    Sprite icon;  // Not meant to be modified in game [only in Editor]
    public Sprite Icon { get => icon; private set => icon = value; }
}
