using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class AppleJuiceBehaviour : ProjectileWeaponBehaviour
{
    private GameObject player;
    public float slowPercent = 0.5f;
    private float TickRate = 0.5f;
    private float Timer;
    private float TickTimer = 0.1f;
    private List<GameObject> playerList;
    private CapsuleCollider2D col;
    [SerializeField] private GameObject appleJuiceBox;
    bool boxDelete = false;

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
        Timer = weaponDuration + 2; // Gives time for the animation to play
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
            timeTakenUp = ScaleUpTransition(timeTakenUp, weaponSize, 0.5f);
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
        if (Timer <= weaponDuration)
        {
            TickTimer -= Time.deltaTime; Debug.Log("ticking");
            if (TickTimer <= 0 && col.enabled == false)
            {
                col.enabled = true; Debug.Log("col enabled");
                TickTimer = TickRate;
            }
            else if (TickRate - TickTimer >= 0.1 && col.enabled == true)
            {
                col.enabled = false; Debug.Log("col disabled");
            }
        }
    }

    void Animating()
    {
        Timer -= Time.deltaTime;
        if (Timer <= weaponDuration + 0.5f && boxDelete == false)
        {
            // Ease out on death
            float t = timeTakenDown / 0.25f;
            appleJuiceBox.transform.localScale = new Vector3(Mathf.Lerp(1, 0, t), Mathf.Lerp(1, 0, t), 1);
            timeTakenDown += Time.deltaTime;

            if (appleJuiceBox.transform.localScale == new Vector3(0, 0, 1))
            {
                boxDelete = true;
                Destroy(appleJuiceBox);
            }
        }
        else if (Timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    new protected void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("collision");
        if (col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<PlayerStats>().RestoreHealth(weaponDamage);
        }
        if (col.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ApplySlow(TickRate, 0.5f);
            }
        }
    }
}
