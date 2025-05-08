using UnityEngine;

public class HitParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem par;
    void Start()
    {
        Destroy(gameObject, 1);
    }
    public void SetValues(Vector2 origin, Vector2 instancePoint, Color Colour, float Size)
    {
        transform.position = instancePoint;
        var main = par.main;
        main.startColor = Colour;
        main.startSize = Size;
        Vector2 angle = origin - instancePoint;
        transform.rotation = Quaternion.Euler(0, 0, 180 - (Mathf.Atan2(angle.x, angle.y) * Mathf.Rad2Deg));
    }
}
