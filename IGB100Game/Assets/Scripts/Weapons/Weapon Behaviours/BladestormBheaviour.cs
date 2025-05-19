using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BladestormBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> markedEnemies;
    public float hitResetInterval = 0.5f; // Time in seconds before enemies can be hit again
    private float timer;

    // Visual Feedback Par
    [SerializeField] private Color parColour;
    [SerializeField] private GameObject par;

    [SerializeField] private AudioClip spawnAudio;

    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
        timer = hitResetInterval;
        SetScale();

        AudioManager.instance.PlaySFX(spawnAudio, transform, 1);
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
        
        if (timer <= 0f)
        {
            markedEnemies.Clear();
            timer = hitResetInterval;
        }

        transform.RotateAround(transform.parent.position, Vector3.forward, 300 * Time.deltaTime);
        
        // EASING STUFFS
        windDownTimer += Time.deltaTime;

        // Ease in on spawn
        if (transform.localScale != new Vector3(weaponSize, weaponSize, 1))
        {
            timeTakenUp = ScaleUpTransition(timeTakenUp, weaponSize, 0.5f);
        }

        // Ease out on death
        if (windDownTimer >= weaponDuration - 0.51)
        {
            timeTakenDown = ScaleDownTransition(timeTakenDown, weaponSize, 0.5f);
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