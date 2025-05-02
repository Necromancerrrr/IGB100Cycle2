using UnityEngine;

public class EnemyZonerStats : EnemyStats
{
    public EnemyZonerScriptableObject zonerData;
    // Current stats
    [HideInInspector]
    public float zoneFrequency;
    [HideInInspector]
    public float zoneCount;
    [HideInInspector]
    public float zoneSize;
    [HideInInspector]
    public float zoneDelay;

    // Zoner logic
    float zoneTimer = 5;

    [SerializeField] GameObject AOE;

    new void Awake()
    {
        base.Awake();
        // Zoner Stats
        zoneFrequency = zonerData.ZoneFrequency;
        zoneCount = zonerData.ZoneCount;
        zoneSize = zonerData.ZoneSize;
        zoneDelay = zonerData.ZoneDelay;
    }
    new void Update()
    {
        base.Update();
        Movement();
        AOEUpdate();
    }
    private void Movement()
    {
        if (knockbackDuration <= 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentMoveSpeed * Time.deltaTime); // Constantly moves towards player
            //Sprite flips towards player
            Vector2 lookDirection = (player.transform.position - transform.position).normalized;
            sr.flipX = lookDirection.x > 0;
        }
    }
    private void AOEUpdate()
    {
        zoneTimer -= Time.deltaTime;
        if (zoneTimer <= 0)
        {
            for (int i = 0; i < zoneCount; i++)
            {
                SpawnAOE();
            }
            zoneTimer = zoneFrequency;
        }
    }
    private void SpawnAOE()
    {
        GameObject instance = Instantiate(AOE);
        instance.transform.position = transform.position; // Assign the position to be the same as this object which is parented to the player
        instance.GetComponent<EnemyAOE>().SetStats(currentDamage, currentMoveSpeed * 3, zoneSize, zoneDelay);
        enemyAudio.PlayEnemyZoneActiveSound();
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage); // Make sure to use currentDamage instead of enemyData.Damage in case of any damage multipliers in the future

        }
    }
}
