using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUIManager : MonoBehaviour
{
    public Image[] heartImages; // Drag 3 UI Image objects here
    public Sprite heart_full;
    public Sprite heart_half;
    public Sprite heart_quarter;
    public Sprite heart_empty;

    private PlayerStats playerStats;
    private float maxHealthPerHeart;

    [SerializeField] private List<GameObject> heartsList;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject heartParent;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        maxHealthPerHeart = playerStats.characterData.MaxHealth / heartImages.Length;
        UpdateHearts();
    }

    void Update()
    {
        UpdateHearts();
    }

    void UpdateHearts()
    {
        float currentHealth = Mathf.Round(playerStats.CurrentHealth);
        for (int i = 1; i <= playerStats.characterData.MaxHealth; i++)
        {
            if (heartsList.Count <= i)
            {
                GameObject heartInstance = Instantiate(heartPrefab);
                heartInstance.transform.SetParent(heartParent.transform, false);
                heartInstance.transform.position = new Vector2(-75 * (i - 1), 0) + (Vector2)heartParent.transform.position;
                heartsList.Add(heartInstance);
            }
            if (currentHealth >= i)
            {
                heartsList[i].GetComponent<Image>().sprite = heart_full;
            }
            else
            {
                heartsList[i].GetComponent<Image>().sprite = heart_empty;
            }
        }

        //for (int i = 0; i < heartImages.Length; i++)
        //{



            // float heartHealth = Mathf.Clamp(currentHealth, 0, maxHealthPerHeart);

            // if (heartHealth >= maxHealthPerHeart)
            //    heartImages[i].sprite = heart_full;
            //else if (heartHealth >= (maxHealthPerHeart * 2f / 3f))
            //    heartImages[i].sprite = heart_half;
            //else if (heartHealth >= (maxHealthPerHeart * 1f / 3f))
            //    heartImages[i].sprite = heart_quarter;
            //else
            //    heartImages[i].sprite = heart_empty;

            //currentHealth -= maxHealthPerHeart;


        //}
    }
}
