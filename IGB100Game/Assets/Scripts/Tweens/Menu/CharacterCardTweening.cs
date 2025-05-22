using UnityEngine;
using DG.Tweening;

public class CharacterCardTweening : MonoBehaviour
{

    private float initialPosition;

    [SerializeField] private float duration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position.y;
    }

    public void TweenYDown()
    {
        transform.DOMoveY(initialPosition - 2, duration, false).SetEase(Ease.OutQuint);
    }

    public void TweenYUp()
    {
        transform.DOMoveY(initialPosition, duration, false).SetEase(Ease.OutQuint);
    }
}
