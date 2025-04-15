using UnityEngine;

public class EnemyBasic : EnemyAbstract
{
    // it moves
    void Update()
    {
        Vector2 angleCalc = player.GetComponent<Rigidbody2D>().position - RB2D.position;
        angleCalc.Normalize();
        RB2D.linearVelocity = angleCalc.normalized * speed;
    }
}
