using UnityEngine;

public class MineBehaviour : ProjectileWeaponBehaviour
{
    [SerializeField] private GameObject damagePrefab;
    Animator anim;
    bool Close;
    float timer; bool speedChange;
    new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        timer = weaponDuration - 5;
        speedChange = false;
        transform.localScale = new Vector3(0.5f + weaponSize / 10, 0.5f + weaponSize / 10, 1);
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
