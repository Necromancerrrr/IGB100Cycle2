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
        weaponDuration = GetCurrentDuration(); Debug.Log("Orbital Duration: " + weaponDuration);
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
            Timer = weaponDuration;
            colli.enabled = true;
            Active = true;
            var main = par.main;
            GameObject.FindWithTag("CineCamera").GetComponent<ScreenShake>().SetShake(weaponSize * 0.5f, weaponDuration + 0.2f);
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
        transform.position = (Vector2)player.transform.position + angle * Random.Range(5f + weaponSize, 20f + weaponSize);
    }
    private void SetScale() // Matches the scale of the collider and VFX to match area size
    {
        colli.radius = weaponSize;
        sprite.size = new Vector2(2.8f * weaponSize, 2.8f * weaponSize);
        var main = par.main;
        main.startSize = 2.8f * weaponSize / 0.3f;
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
