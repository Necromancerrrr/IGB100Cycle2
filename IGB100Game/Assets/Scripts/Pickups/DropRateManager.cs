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

    void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) // Stops an error with spawned objects in play mode (can remove)
        {
            return;
        }

        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();

        foreach (Drops rate in drops) 
        { 
            if(randomNumber <= rate.dropRate)
            {
                possibleDrops.Add(rate);
            }
        }

        //Check if there are possible drops
        if (possibleDrops.Count > 0)
        { 
            Drops drops = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            GameObject spawn = Instantiate(drops.itemPrefab, transform.position, Quaternion.identity);
            spawn.GetComponent<ICollectable>().SetValue(drops.dropValue);
        }
    }
}
