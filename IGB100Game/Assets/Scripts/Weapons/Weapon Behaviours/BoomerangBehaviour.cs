using UnityEngine;

public class BoomerangBehaviour : ProjectileWeaponBehaviour
{
    float initialRangSpeed;
    protected override void Start()
    {
        base.Start();
        initialRangSpeed = currentSpeed;
    }

     void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * 720);
        transform.position += direction * initialRangSpeed * Time.deltaTime; // Set the movement of the knife
        initialRangSpeed -= (Time.deltaTime * 10);
    }
}
