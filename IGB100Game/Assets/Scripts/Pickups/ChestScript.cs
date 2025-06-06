using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static DropRateManager;

public class ChestScript : MonoBehaviour
{
    List<DropRateManager.Drops> drops;
    float SpawnInterval = 0.1f;
    float Timer = 1f;
    bool active = false;

    // scaling up and down transition vars
    public float timeTakenUp = 0f;
    public float timeTakenDown = 0f;
    private void Awake()
    {
        transform.localScale = Vector2.zero;
    }
    public void SetDrops(List<DropRateManager.Drops> dropList)
    {
        drops = dropList;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            // Lerping up
            if (transform.localScale != new Vector3(1, 1, 1))
            {
                timeTakenUp = ScaleUpTransition(timeTakenUp, 1, 0.5f);
            }
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                if (drops.Count > 0)
                {
                    Vector2 randPos = new Vector2(Random.Range(-1f, 1f), 0);
                    Vector2 randAngle = new Vector2(Random.Range(-0.8f, 0.8f), Random.Range(0f, 1f)).normalized;
                    GameObject instance = Instantiate(drops[0].itemPrefab, transform.position + (Vector3)randPos, Quaternion.identity);
                    instance.GetComponent<Rigidbody2D>().linearVelocity = randAngle * Random.Range(20f, 30f);
                    if (instance.GetComponent<ExperiencePickup>() != null)
                    {
                        instance.GetComponent<ExperiencePickup>().SetValue(drops[0].dropValue);
                        instance.GetComponent<ExperiencePickup>().collecting = true;
                    }
                    else if (instance.GetComponent<HealthPickup>() != null)
                    {
                        instance.GetComponent<HealthPickup>().SetValue(drops[0].dropValue);
                        instance.GetComponent<HealthPickup>().collecting = true;
                    }
                    Timer = SpawnInterval;
                    drops.RemoveAt(0);
                }
                else
                {
                    active = false;
                    Destroy(gameObject, 2);
                }
            }
        }
        if (!active)
        {
            // Lerping down
            timeTakenDown = ScaleDownTransition(timeTakenDown, 1, 0.5f);
        }
    }
    public float ScaleUpTransition(float timeTakenU, float scaleMax, float duration)
    {
        float t = timeTakenU / duration;
        transform.localScale = new Vector3(Mathf.Lerp(0, scaleMax, t), Mathf.Lerp(0, scaleMax, t), 1);
        //scaleSpeed += scaleRate;
        timeTakenU += Time.deltaTime;
        return timeTakenU;
    }
    public float ScaleDownTransition(float timeTakenD, float scaleMax, float duration)
    {
        float t = timeTakenD / duration;
        transform.localScale = new Vector3(Mathf.Lerp(scaleMax, 0, t), Mathf.Lerp(scaleMax, 0, t), 1);
        //scaleSpeed += scaleRate;
        timeTakenD += Time.deltaTime;
        return timeTakenD;
    }
}
