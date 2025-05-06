using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class AppleJuiceBehaviour : ProjectileWeaponBehaviour
{
    private GameObject player;
    private float TickRate = 1f;
    private float ListTimer;
    public List<GameObject> playerList;
    new void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player");
        playerList = new List<GameObject>();
    }
    new void Start()
    {
        base.Start();
        SetScale();
        SetPos();
    }
    void SetScale()
    {
        transform.localScale = new Vector3(weaponSize, weaponSize, 1);
    }
    void SetPos()
    {
        Vector2 angle = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // Generate random angle
        transform.position = (Vector2)player.transform.position + angle * Random.Range(5f + weaponSize, 10f + weaponSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerList.Count != 0)
        {
            ListTimer -= Time.deltaTime;
            if (ListTimer <= 0)
            {
                playerList.Clear();
            }
        }
    }
    new protected void OnTriggerEnter2D(Collider2D col)
    {
        // Intentionally left empty
    }
    protected void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && playerList.Contains(col.gameObject) == false)
        {
            col.GetComponent<PlayerStats>().RestoreHealth(weaponDamage);
            playerList.Add(col.gameObject);
            ListTimer = TickRate;
        }
    }
}
