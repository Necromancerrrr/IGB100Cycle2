using UnityEngine;

[CreateAssetMenu(fileName = "EnemyZonerScriptableObject", menuName ="ScriptableObjects/EnemyZoner")]
public class EnemyZonerScriptableObject : ScriptableObject
{
    [Header("Zoner Stats")]
    [SerializeField]
    float zoneFrequency;
    public float ZoneFrequency { get => zoneFrequency; private set => zoneFrequency = value; }

    [SerializeField]
    float zoneCount;
    public float ZoneCount { get => zoneCount; private set => zoneCount = value; }

    [SerializeField]
    float zoneSize;
    public float ZoneSize { get => zoneSize; private set => zoneSize = value; }

    [SerializeField]
    float zoneDelay;
    public float ZoneDelay { get => zoneDelay; private set => zoneDelay = value; }
}
