using UnityEngine;
using UnityEngine.U2D;

public class RotaryBehaviour : ProjectileWeaponBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject projPoint1;
    [SerializeField] private GameObject projPoint2;
    float angle;
    bool clockwise;
    float shootTimer;
    float projectileCount;
    new void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player");
        projectileCount = Mathf.Round(currentProjectileCount * FindFirstObjectByType<PlayerStats>().CurrentProjectileCount);
        shootTimer = 1 / projectileCount;
        projPoint1.GetComponent<Animator>().speed = projectileCount;
        projPoint2.GetComponent<Animator>().speed = projectileCount;
    }
    public void SetStats(float initialAngle, bool clockwiseBool)
    {
        angle = initialAngle;
        clockwise = clockwiseBool;
    }
    void Update()
    {
        Movement();
        Shoot();
        //Anim();
    }
    void Movement()
    {
        if (clockwise) { angle -= Time.deltaTime * currentSpeed; }
        else { angle += Time.deltaTime * currentSpeed; }
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position = player.transform.position;
    }
    void Shoot()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer < 0)
        {
            GameObject projInstance1 = Instantiate(projectile);
            projInstance1.transform.position = projPoint1.transform.position;
            projInstance1.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            projInstance1.GetComponent<RotaryProjectileBehaviour>().SetStats(weaponDamage, currentSpeed/10);
            GameObject projInstance2 = Instantiate(projectile);
            projInstance2.transform.position = projPoint2.transform.position;
            projInstance2.transform.rotation = Quaternion.Euler(0, 0, angle - 280);
            projInstance2.GetComponent<RotaryProjectileBehaviour>().SetStats(weaponDamage, currentSpeed/10);
            shootTimer = 1 / projectileCount;
        }
    }
    private void Anim()
    {
        float angle2 = angle % 360;
        if (angle2 < 0) { angle2 += 360; }
        switch (Mathf.Round(angle2/90))
        {
            case 0:
                projPoint1.GetComponent<Animator>().SetInteger("Direction", 0);
                projPoint1.GetComponent<SpriteRenderer>().flipX = true;
                projPoint2.GetComponent<Animator>().SetInteger("Direction", 2);
                projPoint2.GetComponent<SpriteRenderer>().flipX = false;
                break;
            case 1:
                projPoint1.GetComponent<Animator>().SetInteger("Direction", 1);
                projPoint1.GetComponent<SpriteRenderer>().flipX = false;
                projPoint2.GetComponent<Animator>().SetInteger("Direction", 3);
                projPoint2.GetComponent<SpriteRenderer>().flipX = false;
                break;
            case 2:
                projPoint1.GetComponent<Animator>().SetInteger("Direction", 2);
                projPoint1.GetComponent<SpriteRenderer>().flipX = false;
                projPoint2.GetComponent<Animator>().SetInteger("Direction", 0);
                projPoint2.GetComponent<SpriteRenderer>().flipX = true;
                break;
            case 3:
                projPoint1.GetComponent<Animator>().SetInteger("Direction", 3);
                projPoint1.GetComponent<SpriteRenderer>().flipX = false;
                projPoint2.GetComponent<Animator>().SetInteger("Direction", 1);
                projPoint2.GetComponent<SpriteRenderer>().flipX = false;
                break;
            case 4:
                projPoint1.GetComponent<Animator>().SetInteger("Direction", 0);
                projPoint1.GetComponent<SpriteRenderer>().flipX = true;
                projPoint2.GetComponent<Animator>().SetInteger("Direction", 2);
                projPoint2.GetComponent<SpriteRenderer>().flipX = false;
                break;
        }
    }
}
