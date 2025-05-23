using UnityEngine;

public class EnemyChargeIndicator : MonoBehaviour
{
    private float Duration;
    private float Timer;
    private Vector2 Target;
    private GameObject charger;
    [SerializeField] private GameObject sprite;
    [SerializeField] private Animator anim;
    public void Setup(float Dur, Vector2 Tar, GameObject source, Vector3 scale)
    {
        Target = Tar;
        Duration = Dur;
        Timer = Duration;
        anim.speed = 1 / Duration;
        charger = source;
        transform.localScale = scale;
    }
    
    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        PositionAndRotate();
        if (Timer <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void PositionAndRotate()
    {
        transform.position = charger.transform.position; //Snaps to the correct position
        Vector2 angleVec = ((Vector2)transform.position - Target).normalized; // Finds the normalized difference vector
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(angleVec.y, angleVec.x) * Mathf.Rad2Deg + 90); // Maths. Make angle.
    }
}
