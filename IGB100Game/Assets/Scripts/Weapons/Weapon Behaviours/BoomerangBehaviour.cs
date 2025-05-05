using UnityEngine;

public class BoomerangBehaviour : ProjectileWeaponBehaviour
{
    float rangSpeed;
    bool targetRand;
    bool dir; // True = outwards, false = inwards
    public GameObject target;
    public Vector2 angleVector;
    GameObject player;
    protected override void Start()
    {
        base.Start();
        rangSpeed = currentSpeed;
        dir = true;
        SetScale();
    }
    public void targetSet(bool rand)
    {
        targetRand = rand;
        player = GameObject.FindWithTag("Player");
        SetEnemy();
    }
    void SetScale()
    {
        gameObject.transform.localScale = new Vector3(weaponSize, weaponSize, 0);
    }
    private void SetEnemy() // Selects a random enemy as the target. If there are no valid targets, self destruct.
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
        angleVector = (target.transform.position - player.transform.position).normalized;
    }
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * 720);
        if (dir == true)
        {
            transform.position += (Vector3)angleVector * rangSpeed * Time.deltaTime; // Set the movement of the knife
            rangSpeed -= (Time.deltaTime * 10);
            if (rangSpeed <= 0 || (transform.position - (Vector3)angleVector).magnitude <= 0.2)
            {
                dir = false;
                rangSpeed *= -1;
            }
        }
        else if (dir == false)
        {
            angleVector = (player.transform.position - transform.position).normalized;
            transform.position += (Vector3)angleVector * rangSpeed * Time.deltaTime; // Set the movement of the knife
            rangSpeed += (Time.deltaTime * 10);
            if ((transform.position - player.transform.position).magnitude <= 0.2) { Destroy(gameObject); }
        }
    }
}
