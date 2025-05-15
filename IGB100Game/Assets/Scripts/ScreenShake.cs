using UnityEngine;
using Unity.Cinemachine;
using NUnit.Framework;
using System.Collections.Generic;

public class ScreenShake : MonoBehaviour
{
    // Components
    private CinemachineCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    
    // Logic
    [SerializeField] private List<float> shakeIntensityList;
    [SerializeField] private List<float> shakeDurationList;
    private List<int> removeList = new List<int>();

    private void Awake() // Grabs components
    {
        virtualCamera = GetComponent<CinemachineCamera>();
        perlinNoise = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void SetShake(float intensity, float duration) // Adds values to the two lists
    {
        shakeIntensityList.Add(intensity);
        shakeDurationList.Add(duration);
    }

    private void Update()
    {
        for (int i = 0; i < shakeDurationList.Count; i++) // Subtracts from all timers
        {
            shakeDurationList[i] -= Time.deltaTime;
            if (shakeDurationList[i] <= 0)
            {
                removeList.Add(i); // Marks list values for removal
            }
        }
        for (int i = shakeDurationList.Count; i > -1; i--) // Cycles through the list backwards
        {
            if (removeList.Contains(i)) // Removes any values marked for removal
            {
                shakeIntensityList.RemoveAt(i);
                shakeDurationList.RemoveAt(i);
            }
        }
        foreach (int i in removeList)
        {
            Debug.Log(i);
        }
        removeList.Clear();
        float total = 0;
        foreach(float i in shakeIntensityList)
        {
            total += i;
        }
        perlinNoise.AmplitudeGain = total;
    }
}
