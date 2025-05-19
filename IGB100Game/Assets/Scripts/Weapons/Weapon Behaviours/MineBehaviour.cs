using UnityEngine;

public class MineBehaviour : ProjectileWeaponBehaviour
{
    [SerializeField] private GameObject damagePrefab;
    Animator anim;
    bool Close;
    float timer; bool speedChange;

    Vector3 totalSize;

    [SerializeField] private AudioClip spawnAudio;

    new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        timer = weaponDuration - 5;
        speedChange = false;
        totalSize = new Vector3(0.5f + weaponSize / 10, 0.5f + weaponSize / 10, 1);
        // transform.localScale = totalSize;

        transform.localScale = new Vector3(0, 0, 1);

        AudioManager.instance.PlaySFX(spawnAudio, transform, 1f);
    }

    private void Update()
    {
        if (ClosestCheck().magnitude <= 3 + weaponSize/10)
        {
            anim.SetBool("Close", true);
        }
        else
        {
            anim.SetBool("Close", false);
        }
        timer -= Time.deltaTime;
        if (speedChange == false && timer <= 0)
        {
            speedChange = true;
            anim.speed = 2;
        }

        // EASING STUFFS

        // Ease in on spawn
        if (transform.localScale != totalSize)
        {
            timeTakenUp = ScaleUpTransition(timeTakenUp, 1f, 0.5f);
        }

    }

    private Vector2 ClosestCheck()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length <= 0) { return new Vector2(1000, 1000); }
        else
        {
            GameObject target = enemies[0];
            foreach (GameObject enemy in enemies)
            {
                if ((enemy.transform.position - transform.position).magnitude <= (target.transform.position - transform.position).magnitude)
                {
                        target = enemy;
                }
            }
            return target.transform.position - transform.position;
        }
    }

    protected new void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            GameObject damageInstance = Instantiate(damagePrefab);
            damageInstance.GetComponent<MineDamageBehaviour>().SetStats(transform.position, weaponDamage, weaponSize);
            Destroy(gameObject);
        }
    }
}
