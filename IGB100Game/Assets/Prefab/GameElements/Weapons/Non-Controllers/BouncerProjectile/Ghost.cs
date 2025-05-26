using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject ghost;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(ghostDelaySeconds > 0)
        {
            ghostDelaySeconds -= Time.deltaTime;
        }
        else
        {
            GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
            Destroy(currentGhost, 0.08f);
            ghostDelaySeconds = ghostDelay;
        }
    }
}
