using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // use a queue if this needs to expand to other audio clips
    //Queue enemyHurtQueue = new Queue();

    [SerializeField] private AudioClip EnemyHurt;
    [HideInInspector] public int enemyHurtQueue = 0; // int representing number of times enemyHurt needs to be played

    private AudioSource audioSource;
    private float timer;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (enemyHurtQueue != 0 && timer >= 0.05)
        {
            audioSource.clip = EnemyHurt;
            audioSource.Play();
            enemyHurtQueue -= 1;
            timer = 0;
        }
    }
}
