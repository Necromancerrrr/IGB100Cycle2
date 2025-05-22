using UnityEngine;
using DG.Tweening;

public class UpgradeCard : MonoBehaviour
{
    private float duration = 0.5f;
    private float initialPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScaleUp()
    {
        transform.DOMoveY(initialPosition + 0.5f, duration, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    public void ScaleDown()
    {
        transform.DOMoveY(initialPosition, duration, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }
}
