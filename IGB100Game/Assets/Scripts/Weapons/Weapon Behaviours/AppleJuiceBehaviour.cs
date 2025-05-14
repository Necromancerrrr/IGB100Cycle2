using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class AppleJuiceBehaviour : ProjectileWeaponBehaviour
{
    private GameObject player;
    public float slowPercent = 0.5f;
    private float TickRate = 1f;
    private float Timer;
    private float ListTimer = 1.0f;
    private List<GameObject> playerList;
    private CapsuleCollider2D col;
    [SerializeField] private GameObject appleJuiceBox;
    bool boxDelete = false;
    bool colEnabled = false;

    new void Awake()
    {
        base.Awake();
        player = GameObject.FindWithTag("Player");
        playerList = new List<GameObject>();
        col = GetComponent<CapsuleCollider2D>();
        col.enabled = false;
    }

    new void Start()
    {
        weaponDamage = GetCurrentDamage();
        weaponSize = GetCurrentAreaSize();
        weaponDuration = GetCurrentDuration();
        Timer = weaponDuration + 2;
        SetScale();
        SetPos();
    }

    void SetScale()
    {
        transform.localScale = new Vector3(0, 0, 1);
    }

    void SetPos()
    {
        Vector2 angle = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // Generate random angle
        transform.position = (Vector2)player.transform.position + angle * Random.Range(5f + weaponSize, 10f + weaponSize);
    }

    // Update is called once per frame
    void Update()
    {
        TickCounter();
        Animating();


        // EASING STUFFS
        windDownTimer += Time.deltaTime;

        // Ease in on spawn
        if (transform.localScale != new Vector3(weaponSize, weaponSize, 1))
        {
            scaleUpSpeed = ScaleUpTransition(scaleUpSpeed, 0.004f, weaponSize);
        }

        
        /*
        if (windDownTimer >= weaponDuration - 2)
        {
            appleJuiceBox.transform.localScale = new Vector3(Mathf.Lerp(weaponSize, 0, scaleDownSpeed), Mathf.Lerp(weaponSize, 0, scaleDownSpeed), 1);
            scaleDownSpeed += 0.004f;
        }
        */
    }

    void TickCounter()
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

    void Animating()
    {
        Timer -= Time.deltaTime;
        if (Timer <= weaponDuration + 0.5f && boxDelete == false)
        {
            // Ease out on death
            appleJuiceBox.transform.localScale = new Vector3(Mathf.Lerp(1, 0, scaleDownSpeed), Mathf.Lerp(1, 0, scaleDownSpeed), 1);
            scaleDownSpeed += 0.008f;

            if (appleJuiceBox.transform.localScale == new Vector3(0, 0, 1))
            {
                boxDelete = true;
                Destroy(appleJuiceBox);
            }
        }
        else if (Timer <= weaponDuration && colEnabled == false)
        {
            colEnabled = true;
            col.enabled = true;
        }
        else if (Timer <= 0)
        {
            Destroy(gameObject);
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
        if(col.gameObject.CompareTag("Enemy") && playerList.Contains(col.gameObject) == false) 
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ApplySlow(TickRate, 0.5f);
                playerList.Add(col.gameObject);
                ListTimer = TickRate;
            }
        }
    }
}
