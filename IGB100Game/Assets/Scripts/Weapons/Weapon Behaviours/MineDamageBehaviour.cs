using UnityEngine;

public class MineDamageBehaviour : MonoBehaviour
{
    CircleCollider2D col;
    float weaponDamage;
    float weaponSize;
    float timer = 1;
    int phase = 0;
    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        col.enabled = false;
    }
    public void SetStats(Vector3 position, float damage, float size)
    {
        weaponDamage = damage;
        weaponSize = size;
        transform.position = position;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        transform.localScale = new Vector3(weaponSize, weaponSize, 1);
    }
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; Debug.Log(timer);
        if (phase == 0 && timer <= 0.4)
        { 
            phase = 1;
            col.enabled = true;
            GameObject.FindWithTag("CineCamera").GetComponent<ScreenShake>().SetShake(weaponSize * 10f, 0.4f);
        }
        else if (phase == 1 && timer <= 0.2) 
        {
            phase = 2;
            col.enabled = false;
        }
        else if (phase == 2 && timer <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        // Reference the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(weaponDamage, transform.position, -2 * weaponSize); // Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers in the future
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
