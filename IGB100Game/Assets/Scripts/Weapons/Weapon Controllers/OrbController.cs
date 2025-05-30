using UnityEngine;

public class OrbController : WeaponController
{
    [SerializeField] private AudioClip spawnAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Attack()
    {
        base.Attack();
        AudioManager.instance.PlaySFX(spawnAudio, transform, 1f);
        for (int i = 0; i < weaponCount; i++)
        {
            GameObject orbInstance = Instantiate(weaponData.Prefab);
            orbInstance.transform.position = transform.position; // Assign the position to be the same as this object, which is parented to the player
            if (i == 0)
            {
                orbInstance.GetComponent<OrbBehaviour>().targetSet(false);
            }
            else { orbInstance.GetComponent<OrbBehaviour>().targetSet(true); }
        }
    }
}
