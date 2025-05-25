using UnityEngine;

public class FireballController : WeaponController
{
    [SerializeField] private AudioClip spawnAudio;
    override protected void Start()
    {
        base.Start();
    }
    protected override void Attack()
    {
        base.Attack();
        AudioManager.instance.PlaySFX(spawnAudio, transform, 1);
        for (int i = 0; i < weaponCount; i++)
        {
            GameObject fireballInstance = Instantiate(weaponData.Prefab);
            fireballInstance.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
        }
    }
}
