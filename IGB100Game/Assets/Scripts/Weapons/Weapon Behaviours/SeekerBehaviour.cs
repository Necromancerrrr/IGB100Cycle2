using UnityEngine;

public class SeekerBehaviour : ProjectileWeaponBehaviour 
{
    private GameObject target;
    private Rigidbody2D rb;
    [SerializeField] private ParticleSystem par;
    override protected void Start()
    {
        // Grab the duration of the weapon and multiply it by the modifier
        base.Start();
    }
    private void Update()
    {
        Movement();
    }
    private void Movement()
    {
        if (target == null)
        {
            SetEnemy();
        }
        Vector2 angle = rb.transform.position - target.transform.position;
        rb.linearVelocity = angle * currentSpeed * Time.deltaTime;
    }
    private void SetEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        target = enemies[Random.Range(0, enemies.Length - 1)];
    }
}
