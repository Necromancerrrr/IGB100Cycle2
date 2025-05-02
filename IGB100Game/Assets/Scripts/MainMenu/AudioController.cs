using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip Select;
    [SerializeField] private AudioClip HoverUI;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    public void PlaySelectSound()
    {
        audioSource.clip = Select;
        audioSource.Play();
    }

    public void PlayHoverSound()
    {
        audioSource.clip = HoverUI;
        audioSource.Play();
    }
}
