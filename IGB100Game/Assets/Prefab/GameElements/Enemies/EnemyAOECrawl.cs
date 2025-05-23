using UnityEngine;

public class EnemyAOECrawl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float Timer = 2;
    float timeTakenUp = 0;
    float timeTakenDown = 0;
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        anim.Play("ZonerAOECrawl", 0, Random.Range(0.0f, 1.0f));
        transform.localScale = Vector2.zero;
    }
    private void Update()
    {
        Timer -= Time.deltaTime;
        if (transform.localScale != new Vector3(1, 1, 1))
        {
            float t = timeTakenUp / 0.9f;
            transform.localScale = new Vector3(Mathf.Lerp(0, 1, t), Mathf.Lerp(0, 1, t), 1);
            timeTakenUp += Time.deltaTime;
        }

        // Ease out on death
        if (Timer <= 1.1)
        {
            float t = timeTakenDown / 0.9f;
            transform.localScale = new Vector3(Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), 1);
            timeTakenDown += Time.deltaTime;
        }
        if (Timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
