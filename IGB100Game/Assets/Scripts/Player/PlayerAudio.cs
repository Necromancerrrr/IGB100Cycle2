using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip PlayerHurt;
    public AudioClip PlayerDeath;
    public AudioClip EXPGain;
    public AudioClip HealthGain;

    public AudioSource PlayerAudioSource;

    private int PitchCount = 0;
    public int MaxPitchMult = 9;

    public float pitchTimer = 0.5f;

    public void Awake()
    {
        PlayerAudioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if(pitchTimer > 0)
        {
            pitchTimer -= Time.deltaTime;
        }
        else
        {
            PlayerAudioSource.pitch = 1;
            PlayerAudioSource.volume = 1.0f;
            PitchCount = 0;
        }
    }

    public void PlayPlayerHurtSound()
    {
        AudioManager.instance.PlaySFX(PlayerHurt, transform, 1f);
    }
    public void PlayPlayerDeathSound()
    {
        GameObject music = GameObject.FindGameObjectWithTag("MusicPlayer");
        music.GetComponent<AudioSource>().clip = PlayerDeath;
        music.GetComponent<AudioSource>().loop = false;
        music.GetComponent<AudioSource>().Play();
    }
    public void PlayEXPGainSound()
    {
        if(PlayerAudioSource.time >= PlayerAudioSource.clip.length * 0.3 || !PlayerAudioSource.isPlaying)
        {
            pitchTimer = 1.0f;
            PitchCount++;

            if (PitchCount <= MaxPitchMult)
            {
                for (int i = 0; i < PitchCount; i++)
                {
                    PlayerAudioSource.pitch *= 1.0059463f;
                    PlayerAudioSource.volume *= 0.995f;
                }
            }

            PlayerAudioSource.Play();
        } 
    }
    public void PlayHealthGainSound()
    {
        AudioManager.instance.PlaySFX(HealthGain, transform, 1f);
    }
}
