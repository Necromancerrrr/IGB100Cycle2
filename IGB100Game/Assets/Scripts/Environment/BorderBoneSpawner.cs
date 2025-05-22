using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BorderBoneSpawner : MonoBehaviour
{
    public enum BorderType { Top, Bottom, Left, Right }
    [Tooltip("Which border this is")]
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
            spawnTimer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            playerInside = false;
    }

    private void Update()
    {
        // 1) Cleanup distant bones
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

        // 2) While player’s inside, spawn waves
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
        // Destroy old bones
        foreach (var bone in spawnedBones)
        {
            if (bone != null)
                Destroy(bone);
        }
        spawnedBones.Clear();

        // Get world-space bounds of the border collider
        Bounds b = borderCol.bounds;
        Vector2 spawnLinePoint;
        Vector2 tangent, normal;

        switch (borderType)
        {
            case BorderType.Top:
                spawnLinePoint.y = b.max.y;
                spawnLinePoint.x = Mathf.Clamp(player.position.x, b.min.x, b.max.x);
                tangent = Vector2.right;
                normal = Vector2.down;
                break;

            case BorderType.Bottom:
                spawnLinePoint.y = b.min.y;
                spawnLinePoint.x = Mathf.Clamp(player.position.x, b.min.x, b.max.x);
                tangent = Vector2.right;
                normal = Vector2.up;
                break;

            case BorderType.Left:
                spawnLinePoint.x = b.min.x;
                spawnLinePoint.y = Mathf.Clamp(player.position.y, b.min.y, b.max.y);
                tangent = Vector2.up;
                normal = Vector2.right;
                break;

            case BorderType.Right:
                spawnLinePoint.x = b.max.x;
                spawnLinePoint.y = Mathf.Clamp(player.position.y, b.min.y, b.max.y);
                tangent = Vector2.up;
                normal = Vector2.left;
                break;

            default:
                spawnLinePoint.y = b.max.y;
                spawnLinePoint.x = Mathf.Clamp(player.position.x, b.min.x, b.max.x);
                tangent = Vector2.right;
                normal = Vector2.down;
                break;
        }

        float totalOffset = outwardOffset + anticipationDistance;
        int half = boneCount / 2;

        for (int i = -half; i <= half; i++)
        {
            Vector2 along = tangent * (i * spacing);
            Vector2 spawnPos = spawnLinePoint + along + normal * totalOffset;

            var bone = Instantiate(bonePrefab, spawnPos, Quaternion.identity);
            spawnedBones.Add(bone);

            var anim = bone.GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("PopTrigger");
        }
    }
}