using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // use a queue if this needs to expand to other audio clips
    //Queue enemyHurtQueue = new Queue();

    [SerializeField] private AudioClip EnemyHurt;
    [HideInInspector] public int enemyHurtQueue = 0;

    private AudioSource audioSource;
    private float timer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(enemyHurtQueue);

        timer += Time.deltaTime;
        /*
        while (enemyHurtQueue != 0)
        {
            if (timer >= 0.02)
            {
                audioSource.clip = EnemyHurt;
                audioSource.Play();
                enemyHurtQueue -= 1;
                timer = 0;
            }
        }
        */
        if (enemyHurtQueue != 0 && timer >= 0.05)
        {
            audioSource.clip = EnemyHurt;
            audioSource.Play();
            enemyHurtQueue -= 1;
            timer = 0;
        }
        
    }
}
