using System.Collections.Generic;
using UnityEngine;

public class BouncerBehaviour : ProjectileWeaponBehaviour
{
    // Targetting logic
    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private List<GameObject> hitList;
    [SerializeField] private Vector2 target;
    
    // Components
    Rigidbody2D rb;

    // Sprite anim
    private float rotation;
    private float rotRate;
    private bool clockwise;

    // Visual Feedback Par
    [SerializeField] private Color parColour;
    [SerializeField] private GameObject par;

    new void Start()
    {
        base.Start();
        currentPierce = (int)Mathf.Round(currentPierce * FindFirstObjectByType<PlayerStats>().CurrentProjectileDuration);
        rb = GetComponent<Rigidbody2D>();
        rotation = Random.Range(0f, 360f);
        SetRotSpeed();

        
    }

    void SetRotSpeed()
    {
        rotRate = Random.Range(0f, 50f);
        if (Random.Range(0, 2) == 0) { clockwise = true; }
        else { clockwise = false; }
    }
    private void Update()
    {
        if (clockwise) { rotation += rotRate * Time.deltaTime; }
        else { rotation -= rotRate * Time.deltaTime; }
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
    protected new void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, 0);
            hitList.Add(col.gameObject);
            GameObject parInstance = Instantiate(par);
            parInstance.GetComponent<HitParticle>().SetValues(transform.position, col.transform.position, parColour, 0.5f);
            FindTarget();
            ReducePierce();
            SetRotSpeed();
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(weaponDamage);
            }
            hitList.Add(col.gameObject);
            GameObject parInstance = Instantiate(par);
            parInstance.GetComponent<HitParticle>().SetValues(transform.position, col.transform.position, parColour, 0.5f);
            FindTarget();
            ReducePierce();
            SetRotSpeed();
        }
    }
    void FindTarget()
    {
        enemyList.Clear();
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) // Forms a list of enemies that are alive and have not been hit
        {
            if (hitList.Contains(enemy) == false) 
            {
                enemyList.Add(enemy);
            }
        }
        if (enemyList.Count > 0) // If there is at least 1 enemy alive, select the closest one
        {
            target = enemyList[0].transform.position; // Ensures the previous target is no longer a concern
            foreach (GameObject enemy in enemyList)
            {
                if ((enemy.transform.position - transform.position).magnitude <= ((Vector3)target - transform.position).magnitude)
                {
                    target = enemy.transform.position;
                }
            }
            rb.linearVelocity = (target - (Vector2)transform.position).normalized * currentSpeed;
        }
        else { Destroy(gameObject); }
    }
}
