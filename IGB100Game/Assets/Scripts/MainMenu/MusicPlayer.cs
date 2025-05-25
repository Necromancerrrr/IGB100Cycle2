using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource music;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundFXSlider;
    private void Awake()
    {
        SetSliderValues();
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
    void SetSliderValues()
    {
        if (musicSlider != null)
        {
            audioMixer.GetFloat("musicVol", out float audioLevel);
            if (audioLevel == -80f) { musicSlider.value = 0; }
            else { musicSlider.value = Mathf.Pow(10, audioLevel / 20); }
        }
        if (soundFXSlider != null)
        {
            audioMixer.GetFloat("soundFXVol", out float audioLevel);
            if (audioLevel == -80f) { soundFXSlider.value = 0; }
            else { soundFXSlider.value = Mathf.Pow(10, audioLevel / 20); }
        }
    }
    public void SetMusicVol(float slideValue)
    {
        if (slideValue == 0) { audioMixer.SetFloat("musicVol", -80f); }
        else { audioMixer.SetFloat("musicVol", Mathf.Log10(slideValue) * 20); }
    }
    public void SetSoundFXVol(float slideValue)
    {
        if (slideValue == 0) { audioMixer.SetFloat("soundFXVol", -80f); }
        else { audioMixer.SetFloat("soundFXVol", Mathf.Log10(slideValue) * 20); }
    }
}
