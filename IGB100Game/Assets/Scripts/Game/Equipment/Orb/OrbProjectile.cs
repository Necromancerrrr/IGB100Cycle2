using UnityEngine;

public class OrbProjectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem OrbAura;
    private float damage;
    private float AOESize;
    private float Duration;
    private float Timer = 0;
    public void OnInstantiate(float Dam, float AOE, float Dur)
    {
        damage = Dam; AOESize = AOE; Duration = Dur;
        GetComponent<CircleCollider2D>().radius = AOESize;
        var parti = OrbAura.shape;
        parti.radius = AOESize;
    }
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            // add stuff that makes damage go here
            Timer = 0.5f;
        }
    }
}
