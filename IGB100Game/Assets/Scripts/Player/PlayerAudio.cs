using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip PlayerHurt;
    public AudioClip PlayerDeath;
    public AudioClip EXPGain;
    public AudioClip HealthGain;
    public void PlayPlayerHurtSound()
    {
        AudioSource.PlayClipAtPoint(PlayerHurt, transform.position);
    }
    public void PlayPlayerDeathSound()
    {
        AudioSource.PlayClipAtPoint(PlayerDeath, transform.position);
    }
    public void PlayEXPGainSound()
    {
        AudioSource.PlayClipAtPoint(EXPGain, transform.position);
    }
    public void PlayHealthGainSound()
    {
        AudioSource.PlayClipAtPoint(HealthGain, transform.position);
    }
}
