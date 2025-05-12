using UnityEngine;

public class EnemyDeathParticle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 2)
        {
            Destroy(gameObject);
        }
    }

    void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
