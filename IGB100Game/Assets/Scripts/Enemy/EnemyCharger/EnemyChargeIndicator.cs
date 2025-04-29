using UnityEngine;

public class EnemyChargeIndicator : MonoBehaviour
{
    private float Duration;
    private float Timer;
    private Vector2 Target;
    private ParticleSystem Par;
    private void Awake()
    {
        Par = GetComponent<ParticleSystem>();
    }
    public void Setup(float Dur, Vector2 Tar)
    {
        Target = Tar;
        Duration = Dur;
        Timer = Duration;
        Setup2();
    }
    private void Setup2() // All of the positioning and such that is required
    {
        var shape = Par.shape;
        shape.radius = Vector2.Distance(transform.position, Target)/2; // Set particle system to be the correct size
        var main = Par.main;
        main.startLifetime = Duration * 1.2f;
        Vector2 angleVec = ((Vector2)transform.position - Target).normalized; // Finds the normalized difference vector
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(angleVec.y, angleVec.x) * Mathf.Rad2Deg); // Maths. Make angle.
        transform.position = Vector2.Lerp(transform.position, Target, 0.5f); // Moves the object to the middle of the charger and the target
    }
    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            var emission = Par.emission;
            emission.rateOverTime = 0;
        }
        if (Timer <= Duration * -0.2f)
        {
            Destroy(gameObject);
        }
    }
}
