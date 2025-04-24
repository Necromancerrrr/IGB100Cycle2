using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioClip EnemyHurt;
    public AudioClip EnemyCharge;
    public AudioClip EnemyShoot;
    public AudioClip EnemyZonePrepare;
    public AudioClip EnemyZoneActive;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void PlayEnemyHurtSound()
    {
        AudioSource.PlayClipAtPoint(EnemyHurt, player.transform.position);
    }

    public void PlayEnemyChargeSound()
    {
        AudioSource.PlayClipAtPoint(EnemyCharge, player.transform.position);
    }

    public void PlayEnemyShootSound()
    {
        AudioSource.PlayClipAtPoint(EnemyShoot, player.transform.position);
    }

    public void PlayEnemyZonePrepareSound()
    {
        AudioSource.PlayClipAtPoint(EnemyZonePrepare, player.transform.position);
    }

    public void PlayEnemyZoneActiveSound()
    {
        AudioSource.PlayClipAtPoint(EnemyZoneActive, player.transform.position);
    }
}
