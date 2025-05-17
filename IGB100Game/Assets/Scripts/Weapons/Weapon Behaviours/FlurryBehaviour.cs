using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.GraphicsBuffer;

public class FlurryBehaviour : ProjectileWeaponBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject spriteRenderer;
    private GameObject player;
    private float angle;
    private float timer;
    private float projCount;
    private float phaseCounter;
    private float phase;
    private bool anim;
    new void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player");
        projCount = Mathf.Round(currentProjectileCount * FindFirstObjectByType<PlayerStats>().CurrentProjectileCount);
        timer = 2;
        phaseCounter = 0;
        phase = 0;
        anim = false;
    }

    new void Start()
    {
        weaponDamage = GetCurrentDamage();
        weaponSize = GetCurrentAreaSize();
        weaponDuration = GetCurrentDuration();
        
        transform.localScale = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (phase == 0)
        {
            SetEnemy();
            timer -= Time.deltaTime;
            if (timer <= 0.25 && anim == false) { spriteRenderer.GetComponent<Animator>().SetTrigger("NextAnim"); anim = true; }
            if (timer <= 0) { phase = 1; timer = 1 / projCount; }
        }
        if (phase == 1)
        {
            ShootCheck();
            if (phaseCounter >= projCount) { phase = 2; timer = 1; spriteRenderer.GetComponent<Animator>().SetTrigger("NextAnim"); }
        }
        if (phase == 2)
        {
            timer -= Time.deltaTime; Debug.Log("Phase 2:"); Debug.Log(timer);
            if (timer <= -2.1f) { Destroy(gameObject); }
        }
        
        // EASING STUFFS
        //windDownTimer += Time.deltaTime;

        // Ease in on spawn
        if (transform.localScale != new Vector3(1, 1, 1))
        {
            timeTakenUp = ScaleUpTransition(timeTakenUp, 1f, 0.5f);
        }
    }
    
    private void SetEnemy() // Selects the position of the closest enemy as the target. If there are no valid targets, self destruct
    {
        Vector2 target;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length <= 0) { Destroy(gameObject); }
        else
        {
            target = enemies[0].transform.position;
            foreach (GameObject enemy in enemies)
            {
                if ((enemy.transform.position - transform.position).magnitude <= (target - (Vector2)transform.position).magnitude)
                {
                    target = enemy.transform.position;
                }
            }
            Vector2 calc = target - (Vector2)transform.position;
            angle = 360 - (Mathf.Atan2(calc.x, calc.y) * Mathf.Rad2Deg);
            if ((angle % 360) -180 <= 0) { spriteRenderer.GetComponent<SpriteRenderer>().flipY = true; }
            else if ((angle % 360) - 180 >= 0) { spriteRenderer.GetComponent<SpriteRenderer>().flipY = false; }
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    void ShootCheck()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameObject projInstance = Instantiate(projectile);
            projInstance.transform.position = transform.position;
            projInstance.transform.rotation = Quaternion.Euler(0, 0, angle + Random.Range(-10f, 10f));
            projInstance.GetComponent<FlurryProjectileBehaviour>().SetStats(weaponDamage, currentSpeed);
            timer = 1 / projCount;
            phaseCounter += 1;
        }
    }
}
