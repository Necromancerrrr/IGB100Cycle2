using UnityEngine;

public class BouncerController : WeaponController
{
    [SerializeField] private AudioClip spawnAudio;
    protected override void Attack()
    {
        base.Attack();
        AudioManager.instance.PlaySFX(spawnAudio, transform, 0.2f);
        float angle = 360 / (weaponCount);
        for (int i = 0; i < weaponCount; i++)
        {
            GameObject bounceInstance = Instantiate(weaponData.Prefab);
            bounceInstance.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
            float projSpeed = weaponData.Speed * FindFirstObjectByType<PlayerStats>().CurrentProjectileSpeed;
            bounceInstance.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Mathf.Sin(angle * i), Mathf.Cos(angle * i)).normalized * projSpeed;
        }
    }
}
