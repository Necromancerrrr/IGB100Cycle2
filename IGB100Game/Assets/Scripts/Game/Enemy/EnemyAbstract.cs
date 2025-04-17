using System.Security.Cryptography;
using UnityEngine;

public abstract class EnemyAbstract : MonoBehaviour, IDamageable
{
    // Stats
    [SerializeField] protected float health;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [SerializeField] protected float totalEXP;

    // EXP
    [SerializeField] protected float EXPOrbCount;
    [SerializeField] protected GameObject EXPOrb;

    // Visuals
    [SerializeField] protected GameObject deathFX;
    private float DamageTint = 1;

    // Components
    protected Rigidbody2D RB2D;
    protected GameObject player;
    // Finds player object & rigidbody
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        RB2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        DamageTakenColour();
    }
    // Enemies can take damage and die
    // When dying, they drop experience orbs (with experience split among them evenly) and play a VFX
    // Additionally, taking damage modifies DamageTint which affects enemy colour
    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
        Mathf.Clamp(DamageTint -= 0.5f, 0, 1);
        if (health <= 0) 
        {
            for (int i = 0; i < EXPOrbCount; i++)
            {
                GameObject EXPSpawn = Instantiate(EXPOrb);
                EXPSpawn.GetComponent<ExperienceOrb>().EXP = totalEXP / EXPOrbCount;
            }
            GameObject deathFXSpawn = Instantiate(deathFX);
            Destroy(deathFXSpawn, 1);
            Destroy(gameObject);
        }
    }
    // Sets colour tint based on DamageTint (seen in the method above). Also reduces DamageTint over time.
    void DamageTakenColour()
    {
        Mathf.Clamp(DamageTint += 0.5f * Time.deltaTime, 0, 1);
        GetComponent<SpriteRenderer>().color = new Color(1, DamageTint, DamageTint, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            IDamageable obj = (IDamageable)player;
            obj.TakeDamage(damage);
        }
    }

}
