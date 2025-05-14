using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class ShieldBehaviour : MeleeWeaponBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;

    override protected void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        transform.localScale = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    protected void Update()
    {
        Movement();

        // EASING STUFFS
        windDownTimer += Time.deltaTime;

        // Ease in on spawn
        if (transform.localScale != new Vector3(1, 1, 1))
        {
            scaleUpSpeed = ScaleUpTransition(scaleUpSpeed, 0.004f, 1);
        }

        // Ease out on death
        if (windDownTimer >= weaponDuration - 0.5)
        {
            scaleDownSpeed = ScaleDownTransition(scaleDownSpeed, 0.004f, 1);
        }
    }

    private void Movement()
    {
        rb.linearVelocity = player.GetComponent<PlayerMovement>().lastMovedVector * currentSpeed * -1; // Move away from where the player last moved
        rb.position = Vector2.ClampMagnitude(rb.position - (Vector2)player.transform.position, 2f) + (Vector2)player.transform.position; // Clamp to within 1.5f of the player
    }

    override protected void OnTriggerEnter2D(Collider2D col) // Instead of dealing damage, the shield deals knockback scaling with damage
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, weaponDamage); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
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
