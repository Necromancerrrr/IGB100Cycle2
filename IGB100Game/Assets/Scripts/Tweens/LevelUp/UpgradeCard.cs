using UnityEngine;
using DG.Tweening;

public class UpgradeCard : MonoBehaviour
{
    private float initialPosition;

    [SerializeField] private float duration;

    private UpgradeCardContainer cardContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position.y;

        cardContainer = FindFirstObjectByType<UpgradeCardContainer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TweenYDown()
    {
        if (cardContainer.clickable == true)
        {
            transform.DOMoveY(initialPosition, duration, false).SetEase(Ease.OutQuint).SetUpdate(true);
        }
    }

    public void TweenYUp()
    {
        if (cardContainer.clickable == true)
        {
            transform.DOMoveY(initialPosition + 1f, duration, false).SetEase(Ease.OutQuint).SetUpdate(true);
        }
    }
}
