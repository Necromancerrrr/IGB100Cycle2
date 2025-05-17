using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource music;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Retains itself between scenes
        music = GetComponent<AudioSource>();
        GameObject[] dupe = GameObject.FindGameObjectsWithTag("MusicPlayer");
        foreach (GameObject d in dupe)
        if (d != gameObject)
        {
            if (music.resource == d.GetComponent<AudioSource>().resource)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(d);
            }
        }
    }
}
