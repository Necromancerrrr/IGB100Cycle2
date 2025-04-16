using Unity.VisualScripting;
using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    // EXP granted to player upon collision
    public float EXP;

    // Objects
    private GameObject player;
    private Rigidbody2D RB2D;

    // Movement variables
    [SerializeField] private float trackSpeed = 5;
    [SerializeField] private float spawnSpeed = 5;
    [SerializeField] private float speedLimit = 5;
    // Locates player, sets random velocity
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        RB2D = GetComponent<Rigidbody2D>();
        RB2D.linearVelocity = new Vector2(Random.Range(-spawnSpeed, spawnSpeed), Random.Range(-spawnSpeed, spawnSpeed));
    }
    // Alters velocity to move experience orb towards player
    void FixedUpdate()
    {
        Vector2 angleCalc = player.GetComponent<Rigidbody2D>().position - RB2D.position;
        
        Vector2.ClampMagnitude(RB2D.linearVelocity += angleCalc.normalized * trackSpeed, speedLimit);
    }
    // Grants player experience upon collision and terminates itself
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            player.GetComponent<Player>().ExperienceGain(EXP);
            Destroy(gameObject); 
        }
    }
}
