using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class EnemyAOE : MonoBehaviour
{
    // Zone stats
    float damage;
    [SerializeField] float travelSpeed;
    float zoneSize;
    float zoneDelay;

    // Crawl logic
    [SerializeField] GameObject crawl;
    float crawlTimer = 0;

    // Zone logic
    float timeTakenUp = 0;
    float timeTakenDown = 0;
    float zoneTimer = 5;
    bool active = false;
    bool targetReached = false;

    // Components
    Transform player;
    CircleCollider2D colli;
    Animator anim;
    [SerializeField] Vector2 target;
    void Awake() // Gets components and disables irrelevant ones for now
    {
        colli = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        colli.enabled = false;
        anim.speed = 0;
        player = GameObject.FindWithTag("Player").transform;
        Destroy(gameObject, 15f); // Failsafe
    }
    public void SetStats(float Damage, float Speed, float Size, float Delay) // This is called when the projectile is instantiated
    {
        damage = Damage;
        travelSpeed = Speed;
        zoneSize = Size;
        zoneDelay = Delay;
        zoneTimer = zoneDelay;
        SetPosition();
        SetScale();
    }
    void SetPosition() // Generates an angle, then multiply that by a random magnitude (capping out at area size). Place the object at that point.
    {
        target = (Vector2)player.position + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(0f, zoneSize * 3);
    }
    void SetScale() // Increase the scale of the object to match the size. Alter the lifetime of the VFX to match the delay.
    {
        gameObject.transform.localScale = Vector2.zero;
    }
    void Update() // Enables the collider at once the delay ends, then disables the collider after 0.1f.
    {
        if (targetReached == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, travelSpeed * Time.deltaTime); 
            crawlTimer -= Time.deltaTime;
            if (crawlTimer < 0)
            {
                crawlTimer = Random.Range(0.1f, 0.3f);
                Vector2 offset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(0f, 2f);
                Instantiate(crawl, transform.position + (Vector3)offset, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
            }
            if ((Vector2)transform.position == target)
            {
                targetReached = true;
                anim.speed = 1/zoneDelay;
            }
        }
        else
        {
            zoneTimer -= Time.deltaTime;
            if (transform.localScale != new Vector3(zoneSize, zoneSize, 1))
            {
                float t = timeTakenUp / (zoneDelay - 0.5f);
                transform.localScale = new Vector3(Mathf.Lerp(0, zoneSize, t), Mathf.Lerp(0, zoneSize, t), 1);
                timeTakenUp += Time.deltaTime;
            }
            if (zoneTimer <= 0 && active == false)
            {
                colli.enabled = true;
                active = true;
            }
            if (zoneTimer <= -0.1f)
            {
                colli.enabled = false;
                float t = timeTakenDown / 0.8f;
                transform.localScale = new Vector3(Mathf.Lerp(zoneSize, 0, t), Mathf.Lerp(zoneSize, 0, t), 1);
                timeTakenDown += Time.deltaTime;
            }
            if (zoneTimer <= -1)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(damage); // Make sure to use currentDamage instead of enemyData.Damage in case of any damage multipliers in the future
            colli.enabled = false;
        }
    }
}
