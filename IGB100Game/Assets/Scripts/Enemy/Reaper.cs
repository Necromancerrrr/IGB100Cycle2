using UnityEngine;

public class Reaper : EnemyStats
{
    // Reaper Logic
    float SpeedIncreaseTimer = 5;
    float CurrentSize = 1;
    float Scale = 1;
    float scaleSpeed = 0;
    private Animator anim;
    [SerializeField] AudioClip bell;
    new void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        SpawnRoutine();
    }
    void SpawnRoutine()
    {
        GameObject.FindWithTag("CineCamera").GetComponent<ScreenShake>().SetShake(80, 1); // Shakes screen
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) // Deletes all enemies other than itself
        {
            if (enemy != this.gameObject)
            {
                Destroy(enemy);
            }
        }
    }
    new void Update()
    {
        base.Update();
        Movement();
        SpeedIncrease();
        SizeIncrease();
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
    private void SpeedIncrease()
    {
        SpeedIncreaseTimer -= Time.deltaTime;
        if (SpeedIncreaseTimer < 0)
        {
            AudioManager.instance.PlaySFX(bell, transform, 1f);
            currentMoveSpeed += 0.2f;
            anim.speed += 0.4f;
            scaleSpeed = 0;
            CurrentSize = Scale;
            Scale += 1f;
            SpeedIncreaseTimer = 5;
        }   
    }
    private void SizeIncrease()
    {
        transform.localScale = new Vector3(Mathf.Lerp(CurrentSize, Scale, scaleSpeed), Mathf.Lerp(CurrentSize, Scale, scaleSpeed), 1);
        scaleSpeed += 0.004f;
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
