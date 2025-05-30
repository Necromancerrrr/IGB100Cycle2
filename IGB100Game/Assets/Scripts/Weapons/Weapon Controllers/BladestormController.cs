using UnityEngine;

public class BladestormController : WeaponController
{
    [SerializeField] private int numberOfBlades = 4;
    [SerializeField] private float radius = 1.5f;
    float weaponSize;

    protected override void Start()
    {
        base.Start();
        
    }

    protected override void Attack()
    {

        base.Attack();

        weaponSize = weaponData.AreaSize * FindFirstObjectByType<PlayerStats>().CurrentAOE;
        numberOfBlades = Mathf.RoundToInt(weaponData.ProjectileCount * FindFirstObjectByType<PlayerStats>().CurrentProjectileCount);
        radius = weaponSize;

        float angleStep = 360f / numberOfBlades;

        for (int i = 0; i < numberOfBlades; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            Vector3 spawnPosition = transform.position + offset;

            GameObject blade = Instantiate(weaponData.Prefab, spawnPosition, Quaternion.identity);
            blade.transform.parent = transform; // Attach to player so they orbit
            // blade.transform.localScale = new Vector3();

            // rotate the blade to face outward 
            blade.transform.up = offset.normalized;
        }
    }
}
