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
    [SerializeField] private ParticleSystem par;

    // Orbital logic
    private float Timer = 10;
    private bool Active = false;
    private float TickRate = 0.2f;
    private float ListTimer;
    private List<GameObject> EnemyList;
    new void Start()
    {
        weaponDamage = GetCurrentDamage();
        player = GameObject.FindWithTag("Player");
        colli = GetComponent<CircleCollider2D>();
        colli.enabled = false;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        ListTimer = TickRate;
        SetPosition();
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
        if (Timer <= 0 && Active == false)
        {
            anim.SetBool("Active", true);
            Timer = currentDuration;
            colli.enabled = true;
            Active = true;
            var main = par.main;
        }
        else if (Timer <= 0 && Active == true)
        {
            Destroy(gameObject);
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
    private void SetPosition() // Randomly places Orbital in an area somewhat close to the player
    {
        Vector2 angle = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // Generate random angle
        transform.position = (Vector2)player.transform.position + angle * Random.Range(5f + currentAreaSize, 20f + currentAreaSize);
    }
    private void SetScale() // Matches the scale of the collider and VFX to match area size
    {
        colli.radius = currentAreaSize;
        sprite.size = new Vector2(2.8f * currentAreaSize, 2.8f * currentAreaSize);
        var main = par.main;
        main.startSize = 2.8f * currentAreaSize / 0.3f;
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
