using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector instance;
    public CharacterData characterData;

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

    public static CharacterData GetData()
    {
       if(instance && instance.characterData)
            return instance.characterData;

        else
        {
            CharacterData[] characters = Resources.FindObjectsOfTypeAll<CharacterData>();
            if(characters.Length > 0)
            {
                return characters[Random.Range(0, characters.Length)];
            }
        }

       return null;
    }

    public void SelectCharacter(CharacterData character)
    {
        characterData = character;
    }

    public void DestorySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
