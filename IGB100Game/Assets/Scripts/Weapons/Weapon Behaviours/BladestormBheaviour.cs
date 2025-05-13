using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BladestormBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> markedEnemies;
    public float hitResetInterval = 0.5f; // Time in seconds before enemies can be hit again
    private float timer;
    private float windDownTimer;
    private float scaleUpSpeed = 0f;
    private float scaleDownSpeed = 0f;

    // Visual Feedback Par
    [SerializeField] private Color parColour;
    [SerializeField] private GameObject par;
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
        timer = hitResetInterval;
        SetScale();
    }
    void SetScale()
    {
        // (weaponSize, weaponSize, 1)
        transform.localScale = new Vector3(0, 0, 1);
    }
    void Update()
    {
        // Clear marked enemies after the reset interval
        timer -= Time.deltaTime;
        windDownTimer += Time.deltaTime;
        if (timer <= 0f)
        {
            markedEnemies.Clear();
            timer = hitResetInterval;
        }

        transform.RotateAround(transform.parent.position, Vector3.forward, 300 * Time.deltaTime);
        
        if (transform.localScale != new Vector3(weaponSize, weaponSize, 1))
        {
            transform.localScale = new Vector3(Mathf.Lerp(0, weaponSize, scaleUpSpeed), Mathf.Lerp(0, weaponSize, scaleUpSpeed), 1);
            scaleUpSpeed += 0.004f;
        }

        if (windDownTimer >= weaponDuration - 0.5)
        {
            transform.localScale = new Vector3(Mathf.Lerp(weaponSize, 0, scaleDownSpeed), Mathf.Lerp(weaponSize, 0, scaleDownSpeed), 1);
            scaleDownSpeed += 0.004f;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && !markedEnemies.Contains(col.gameObject))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(weaponDamage, transform.position, 5f);
                markedEnemies.Add(col.gameObject);
                GameObject parInstance = Instantiate(par);
                parInstance.GetComponent<HitParticle>().SetValues(transform.position, col.transform.position, parColour, 0.5f);
            }
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable) && !markedEnemies.Contains(col.gameObject))
            {
                breakable.TakeDamage(weaponDamage);
                markedEnemies.Add(col.gameObject);
            }
            GameObject parInstance = Instantiate(par);
            parInstance.GetComponent<HitParticle>().SetValues(transform.position, col.transform.position, parColour, 0.5f);
        }
    }
}