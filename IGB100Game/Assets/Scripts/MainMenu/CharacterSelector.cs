using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector instance;
    public CharacterScriptableObject characterData;

    void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Extra Character Selector");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static CharacterScriptableObject GetData()
    {
        return instance.characterData;
    }

    public void SelectCharacter(CharacterScriptableObject character)
    {
        characterData = character;
    }

    public void DestorySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
