using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class ShieldBehaviour : MeleeWeaponBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private List<GameObject> hitList = new List<GameObject>();
    private float hitTimer = 0.1f;
    float scale = 1;

    [SerializeField] private AudioClip spawnAudio;

    override protected void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        transform.localScale = new Vector3(0, 0, 1);

        AudioManager.instance.PlaySFX(spawnAudio, transform, 1f);
    }

    // Update is called once per frame
    protected void Update()
    {
        Movement();

        // EASING STUFFS
        windDownTimer += Time.deltaTime;

        // Ease in on spawn
        if (transform.localScale != new Vector3(scale, scale, 1))
        {
            timeTakenUp = ScaleUpTransition(timeTakenUp, scale, 0.5f);
        }

        // Ease out on death
        if (windDownTimer >= weaponDuration - 0.51)
        {
            timeTakenDown = ScaleDownTransition(timeTakenDown, scale, 0.5f);
        }

        hitTimer -= Time.deltaTime;
        if (hitTimer <= 0)
        {
            hitTimer = 0.1f;
            hitList.Clear();
        }
    }
    public void IncreaseScale(float value)
    {
        if (weaponData.Level <= 3) { Mathf.Clamp(scale += value, 1f, 2f); }
        if (weaponData.Level <= 4) { Mathf.Clamp(scale += value * 2, 1f, 4f); }
    }
    private void Movement()
    {
        rb.linearVelocity = player.GetComponent<PlayerMovement>().lastMovedVector * currentSpeed * -1; // Move away from where the player last moved
        rb.position = Vector2.ClampMagnitude(rb.position - (Vector2)player.transform.position, 2f) + (Vector2)player.transform.position; // Clamp to within 1.5f of the player
    }

    override protected void OnTriggerEnter2D(Collider2D col)
    {
        // Intentionally kept empty
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && hitList.Contains(col.gameObject) == false)
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, weaponDamage); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
            IncreaseScale(0.02f);
            hitList.Add(col.gameObject);
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(weaponDamage);
            }
        }
    }
}
