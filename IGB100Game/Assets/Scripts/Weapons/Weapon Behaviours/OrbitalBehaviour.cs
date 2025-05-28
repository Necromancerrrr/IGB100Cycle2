using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class OrbitalBehaviour : MeleeWeaponBehaviour
{
    // Component
    private GameObject player;
    private CircleCollider2D colli;
    private Animator anim;
    private SpriteRenderer sprite;

    // Orbital logic
    private int phase = 0;
    private float Timer = 4;
    private float TickRate = 0.2f;
    private float ListTimer;
    private List<GameObject> EnemyList;
    new void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player");
        colli = GetComponent<CircleCollider2D>();
        colli.enabled = false;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        ListTimer = TickRate;
    }
    new void Start()
    {
        weaponDamage = GetCurrentDamage();
        weaponSize = GetCurrentAreaSize();
        weaponDuration = GetCurrentDuration();
        SetScale();
    }
    protected void Update()
    {
        OrbitalPhaseUpdate();
        EnemyListClear();
    }
    private void OrbitalPhaseUpdate() // After 10 seconds, turns on the collider. After a duration later, destroy the object
    {
        Timer -= Time.deltaTime / currentSpeed;
        if (phase == 0) // Tracks player while winding up
        { 
            transform.position = player.transform.position;
            if (transform.localScale != new Vector3(weaponSize, weaponSize, 1))
            {
                float t = timeTakenUp / 0.5f;
                transform.localScale = new Vector3(Mathf.Lerp(0, weaponSize, t), Mathf.Lerp(0, weaponSize, t), 1);
                timeTakenUp += Time.deltaTime;
            }
        } 
        if (Timer <= 0 && phase == 0)
        {
            phase = 1;
            Timer = 1;
            anim.SetBool("Active", true);
        }
        else if (Timer <= 0 && phase == 1)
        {
            phase = 2;
            Timer = weaponDuration;
            colli.enabled = true;
            GameObject.FindWithTag("CineCamera").GetComponent<ScreenShake>().SetShake(weaponSize * 0.15f + 0.5f, weaponDuration + 0.2f);
        }
        else if (Timer <= 0.1 && phase == 2)
        {
            float t = timeTakenDown / 0.2f;
            transform.localScale = new Vector3(Mathf.Lerp(weaponSize, 0, t), Mathf.Lerp(weaponSize, 0, t), 1);
            timeTakenDown += Time.deltaTime;
        }
        else if (Timer <= 0 && phase == 2)
        {
            Destroy(gameObject, 0.2f);
        }
    }
    private void EnemyListClear() // Clears the list of enemies that have already been hit by the orbital
    {
        ListTimer -= Time.deltaTime;
        if (ListTimer <= 0)
        {
            EnemyList = new List<GameObject>();
            ListTimer = TickRate;
        }
    }
    private void SetScale() // Matches the scale of the collider and VFX to match area size
    {
        transform.localScale = new Vector3(0, 0, 1);
    }
    protected void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && EnemyList.Contains(col.gameObject) == false) // Only hits enemies not on the list
        {
            EnemyList.Add(col.gameObject);
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, -5f); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(weaponDamage);
            }
        }
    }
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        // Kept empty intentionally
    }
}
