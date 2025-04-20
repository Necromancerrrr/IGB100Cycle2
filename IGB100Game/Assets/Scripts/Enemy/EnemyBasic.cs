using UnityEngine;

public class EnemyBasic : EnemyAbstract
{
    // it moves
    void Update()
    {
        LookAtPlayer();
        MoveToPlayer();
        
    }

    public void MoveToPlayer()
    {
        Vector2 angleCalc = player.GetComponent<Rigidbody2D>().position - RB2D.position;
        angleCalc.Normalize();
        RB2D.linearVelocity = angleCalc.normalized * speed;
    }

    // Face direction of player
    public void LookAtPlayer()
    {
        if (player.transform.position.x - transform.position.x <= 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
