using UnityEngine;

public class BoomerangBehaviour : ProjectileWeaponBehaviour
{
    float rangSpeed;
    bool targetRand;
    bool dir; // True = outwards, false = inwards
    private GameObject target;
    private Vector2 angleVector;
    GameObject player;

    // HitFX values
    [SerializeField] private Color parColour;
    [SerializeField] private GameObject par;

    bool trailOn = false;
    [SerializeField] private GameObject trail1;
    [SerializeField] private GameObject trail2;
    protected override void Start()
    {
        base.Start();
        rangSpeed = currentSpeed;
        dir = true;
        SetScale();
        var emission1 = trail1.GetComponent<ParticleSystem>().emission;
        emission1.enabled = false;
        var emission2 = trail2.GetComponent<ParticleSystem>().emission;
        emission2.enabled = false;
    }

    public void targetSet(bool rand) // Checks whether the targeting is for the closest enemy or random
    {
        targetRand = rand;
        player = GameObject.FindWithTag("Player");
        SetEnemy();
    }

    void SetScale()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 1);
    }

    private void SetEnemy() // Selects an enemy as the target. If there are no valid targets, self destruct.
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length <= 0) { Destroy(gameObject); }
        else if (targetRand == true)
        {
            target = enemies[Random.Range(0, enemies.Length - 1)];
        }
        else if (targetRand == false)
        {
            target = enemies[0];
            foreach (GameObject enemy in enemies)
            {
                Debug.Log(player);
                if ((enemy.transform.position - player.transform.position).magnitude <= (target.transform.position - player.transform.position).magnitude)
                {
                    target = enemy;
                }
            }
        }
        angleVector = (target.transform.position - player.transform.position).normalized; // The vector we will use for movement on the way out
    }

    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * 720);
        if (dir == true) // Moving outwards
        {
            transform.position += (Vector3)angleVector * rangSpeed * Time.deltaTime;
            rangSpeed -= (Time.deltaTime * 10);
            if (rangSpeed <= 0 || (transform.position - (Vector3)angleVector).magnitude <= 0.2) // Switches to moving back towards the player when reaching 0 speed or when reaching target
            {
                dir = false;
                rangSpeed *= -1;
            }
        }
        else if (dir == false) 
        {
            angleVector = (player.transform.position - transform.position).normalized;
            if (rangSpeed >= 0 && trailOn == false)
            {
                var emission1 = trail1.GetComponent<ParticleSystem>().emission;
                emission1.enabled = true;
                var emission2 = trail2.GetComponent<ParticleSystem>().emission;
                emission2.enabled = true;
                trailOn = true;
                trail1.transform.localScale = new Vector3(weaponSize, weaponSize, 1);
                trail2.transform.localScale = new Vector3(weaponSize, weaponSize, 1);
            }
            transform.position += (Vector3)angleVector * rangSpeed * Time.deltaTime; // Set the movement of the knife
            rangSpeed += (Time.deltaTime * 10);
            if ((transform.position - player.transform.position).magnitude <= 0.2) // Boomerang dies when close to the player
            {
                transform.DetachChildren();
                var emission1 = trail1.GetComponent<ParticleSystem>().emission;
                emission1.enabled = false;
                var emission2 = trail2.GetComponent<ParticleSystem>().emission;
                emission2.enabled = false;
                Destroy(trail1, 1);
                Destroy(trail2, 1);
                Destroy(gameObject); 
            } 
        }

        // EASING STUFFS
        //windDownTimer += Time.deltaTime;

        // Ease in on spawn
        if (transform.localScale != new Vector3(weaponSize, weaponSize, 1))
        {
            timeTakenUp = ScaleUpTransition(timeTakenUp, weaponSize, 0.25f);
        }
    }


    new protected void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            if (dir == false && rangSpeed >= 0) { enemy.TakeDamage(weaponDamage * 2, transform.position, 0); } // enhanced damage for the way back in
            else { enemy.TakeDamage(weaponDamage, transform.position, 0); } // normal damage
            GameObject parInstance = Instantiate(par);
            parInstance.GetComponent<HitParticle>().SetValues(transform.position, col.transform.position, parColour, 0.5f);
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(weaponDamage);
            }
            GameObject parInstance = Instantiate(par);
            parInstance.GetComponent<HitParticle>().SetValues(transform.position, col.transform.position, parColour, 0.5f);
        }
    }
}
