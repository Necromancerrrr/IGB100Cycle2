using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BorderBoneSpawner : MonoBehaviour
{
    public enum BorderType { Top, Bottom, Left, Right }
    public BorderType borderType;

    [Tooltip("Bone prefab with pop animation setup")]
    public GameObject bonePrefab;

    [Tooltip("How many bones to spawn in front of the player each wave")]
    public int boneCount = 5;

    [Tooltip("Spacing between each bone along the border")]
    public float spacing = 1f;

    [Tooltip("Base offset from the border collider")]
    public float outwardOffset = 0.1f;

    [Tooltip("Extra distance to spawn ahead of the trigger")]
    public float anticipationDistance = 1.0f;

    [Tooltip("Max distance from player before a bone despawns")]
    public float maxBoneDistance = 150f;

    [Tooltip("Seconds between spawn waves while player remains in zone")]
    public float spawnInterval = 1f;

    private List<GameObject> spawnedBones = new List<GameObject>();
    private Transform player;
    private Collider2D borderCol;
    private bool playerInside = false;
    private float spawnTimer = 0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        borderCol = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerInside = true;
            spawnTimer = 0f; // immediate spawn
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            playerInside = false;
    }

    private void Update()
    {
        // Cleanup bones too far away
        for (int i = spawnedBones.Count - 1; i >= 0; i--)
        {
            var bone = spawnedBones[i];
            if (bone == null)
            {
                spawnedBones.RemoveAt(i);
                continue;
            }
            if (Vector2.Distance(player.position, bone.transform.position) > maxBoneDistance)
            {
                Destroy(bone);
                spawnedBones.RemoveAt(i);
            }
        }

        // Spawn waves while player is inside
        if (playerInside)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                SpawnBones();
                spawnTimer = spawnInterval;
            }
        }
    }

    private void SpawnBones()
    {
        // Determine the world-space line point on the border edge
        Bounds b = borderCol.bounds;
        Vector2 linePoint;
        Vector2 tangent, normal;

        switch (borderType)
        {
            case BorderType.Top:
                linePoint.y = b.max.y;
                linePoint.x = Mathf.Clamp(player.position.x, b.min.x, b.max.x);
                tangent = Vector2.right; normal = Vector2.down;
                break;
            case BorderType.Bottom:
                linePoint.y = b.min.y;
                linePoint.x = Mathf.Clamp(player.position.x, b.min.x, b.max.x);
                tangent = Vector2.right; normal = Vector2.up;
                break;
            case BorderType.Left:
                linePoint.x = b.min.x;
                linePoint.y = Mathf.Clamp(player.position.y, b.min.y, b.max.y);
                tangent = Vector2.up; normal = Vector2.right;
                break;
            case BorderType.Right:
                linePoint.x = b.max.x;
                linePoint.y = Mathf.Clamp(player.position.y, b.min.y, b.max.y);
                tangent = Vector2.up; normal = Vector2.left;
                break;
            default:
                linePoint.y = b.max.y;
                linePoint.x = Mathf.Clamp(player.position.x, b.min.x, b.max.x);
                tangent = Vector2.right; normal = Vector2.down;
                break;
        }

        float totalOffset = outwardOffset + anticipationDistance;
        int half = boneCount / 2;
        float minDist = spacing * 0.5f;

        for (int i = -half; i <= half; i++)
        {
            Vector2 spawnPos = linePoint + tangent * (i * spacing)
                                     + normal * totalOffset;

            // Check for a nearby existing bone
            GameObject nearby = null;
            foreach (var bObj in spawnedBones)
            {
                if (bObj != null &&
                    Vector2.Distance(bObj.transform.position, spawnPos) < minDist)
                {
                    nearby = bObj;
                    break;
                }
            }

            if (nearby != null)
            {
                // Just update its draw order; don't replay the animation
                var srExist = nearby.GetComponent<SpriteRenderer>();
                if (srExist != null)
                    srExist.sortingOrder = Mathf.RoundToInt(-nearby.transform.position.y * 100);
                continue;
            }

            // No bone here yet → spawn a fresh one
            var bone = Instantiate(bonePrefab, spawnPos, Quaternion.identity);
            spawnedBones.Add(bone);

            // Play its pop animation once
            var boneAnim = bone.GetComponent<Animator>();
            if (boneAnim != null)
                boneAnim.Play("Bone", 0, 0f);

            // And set its draw order so lower bones render in front
            var sr = bone.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sortingOrder = Mathf.RoundToInt(-spawnPos.y * 100);
        }
    }
}