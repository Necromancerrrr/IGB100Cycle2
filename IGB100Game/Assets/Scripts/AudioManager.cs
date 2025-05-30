using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public AudioMixerGroup mix;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [SerializeField] private AudioSource SFXObject;

    // use a queue if this needs to expand to other audio clips
    //Queue enemyHurtQueue = new Queue();

    [SerializeField] private AudioClip EnemyHurt;
    [HideInInspector] public int enemyHurtQueue = 0; // int representing number of times enemyHurt needs to be played

    [SerializeField] public AudioClip LevelUpEnd; // these should be in UpgradeCard.cs but i put them here cus i forgor and i'm too lazy to fix it
    [SerializeField] public AudioClip LevelUpCardHover; // this one too

    private AudioSource audioSource;
    private float timer;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (enemyHurtQueue > 10)
        {
            enemyHurtQueue = 10;
        }

        timer += Time.deltaTime;
        if (enemyHurtQueue != 0 && timer >= 0.05)
        {
            audioSource.clip = EnemyHurt;
            audioSource.Play();
            enemyHurtQueue -= 1;
            timer = 0;
        }
    }

    public void PlaySFX(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource generatedAudioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);

        generatedAudioSource.clip = audioClip;

        generatedAudioSource.volume = volume;

        generatedAudioSource.outputAudioMixerGroup = mix;

        generatedAudioSource.Play();

        float audioLength = generatedAudioSource.clip.length;

        Destroy(generatedAudioSource.gameObject, audioLength);
    }
}
