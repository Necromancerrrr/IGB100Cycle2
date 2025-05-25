using UnityEngine;

public class SeekerController : WeaponController
{
    [SerializeField] private AudioClip spawnAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
    }
    protected override void Attack()
    {
        base.Attack();
        AudioManager.instance.PlaySFX(spawnAudio, transform, 0.3f);
        for (int i = 0; i < weaponCount; i++)
        {
            GameObject seekerInstance = Instantiate(weaponData.Prefab);
            seekerInstance.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
        }
    }
}
