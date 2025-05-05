using UnityEngine;
using Unity.Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineCamera virtualCamera;
    private float shakeTimer;

    private void Awake()
    {
        Instance = this;
        virtualCamera = GetComponent<CinemachineCamera>();
    }

    public void ShakeCamera(float intensity, float duration)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.AmplitudeGain = intensity;
        shakeTimer = duration;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                //Timer over!
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0f;
            }
        }
    }
}
