using UnityEngine;

public class BladestormWeapon : Weapon
{

    [SerializeField] private int numberOfBlades = 4;
    [SerializeField] private float radius = 1.5f;
    float weaponSize;


    protected override void Start()
    {
        base.Start();
        radius = currentStats.area;
        Attack();
        
    }

    public override bool CanAttack()
    {
        return base.CanAttack();
    }
    protected override bool Attack(int attackCount = 1)
    {
        base.Attack();

        Debug.Log("Bladestorm Start");

        float angleStep = 360f / numberOfBlades;

        for (int i = 0; i < numberOfBlades; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            Vector3 spawnPosition = transform.position + offset;

            GameObject blade = Instantiate(currentStats.prefab, spawnPosition, Quaternion.identity);
            blade.transform.parent = transform; // Attach to player so they orbit
            // blade.transform.localScale = new Vector3();

            // rotate the blade to face outward 
            blade.transform.up = offset.normalized;
        }

        currentCooldown += currentStats.cooldown;

        return true;
    }
}
