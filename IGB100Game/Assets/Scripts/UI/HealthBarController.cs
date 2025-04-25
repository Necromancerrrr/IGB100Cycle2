using System.Collections.Generic;
using System;
using NUnit.Framework;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public GameObject heartPrefab;
    List<HealthBarScript> hearts = new List<HealthBarScript>();

    PlayerStats playerStats;

    public float health;
    public float maxHealth;

    private void OnEnable()
    {
        PlayerStats.OnPlayerDamaged += DrawHearts;
        Debug.Log("THIS GOT CALLED " + health.ToString());
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerDamaged -= DrawHearts;
    }

    private void Awake()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
        
    }

    private void Start()
    {
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();
        maxHealth = playerStats.characterData.MaxHealth;

        float maxHealthRemainder = maxHealth % 4;
        int heartsToMake = (int)(maxHealth / 4 + maxHealthRemainder);


        for (int i = 0; i < heartsToMake; i++) 
        { 
            CreateEmptyHeart();
        }

        health = playerStats.CurrentHealth;

        for (int i = 0; i < hearts.Count; i++) {

            int heartStatusRemainder = (int)Mathf.Clamp(health - (i * 4), 0, 4);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthBarScript heartComponent = newHeart.GetComponent<HealthBarScript>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach (Transform t in transform) 
        { 
            Destroy(t.gameObject);
        }
        hearts = new List<HealthBarScript> ();
    }
}
