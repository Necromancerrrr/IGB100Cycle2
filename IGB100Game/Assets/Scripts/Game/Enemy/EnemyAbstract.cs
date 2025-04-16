using UnityEngine;

public abstract class EnemyAbstract : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] protected float totalEXP;
    [SerializeField] protected float EXPOrbCount;
    [SerializeField] protected GameObject EXPOrb;
    [SerializeField] protected GameObject deathFX;
    protected Rigidbody2D RB2D;
    protected GameObject player;
    // Finds player object & rigidbody
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        RB2D = GetComponent<Rigidbody2D>();
    }
    // Enemies can take damage and drop experience
    public void TakeDamage(float incomingDamage)
    {
        health -= incomingDamage;
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
}
