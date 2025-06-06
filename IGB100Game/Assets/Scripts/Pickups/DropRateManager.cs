using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
        public int dropValue;
    }

    public List<Drops> drops;
    public bool BossChest = false;
    [SerializeField] GameObject Chest;

    void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) // Stops an error with spawned objects in play mode (can remove)
        {
            return;
        }
        if (BossChest == false)
        {
            float randomNumber = UnityEngine.Random.Range(0f, 100f);
            List<Drops> possibleDrops = new List<Drops>();

            foreach (Drops rate in drops)
            {
                if (randomNumber <= rate.dropRate)
                {
                    possibleDrops.Add(rate);
                }
            }

            //Check if there are possible drops
            if (possibleDrops.Count > 0)
            {
                Drops drops = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
                Vector3 dropPos = (Vector2)transform.position + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                GameObject spawn = Instantiate(drops.itemPrefab, dropPos, Quaternion.identity);
                spawn.GetComponent<ICollectable>().SetValue(drops.dropValue);
            }
        }
        else if (BossChest == true)
        {
            GameObject chestInstance = Instantiate(Chest, transform.position, Quaternion.identity);
            chestInstance.GetComponent<ChestScript>().SetDrops(drops);
        }
    }
}
