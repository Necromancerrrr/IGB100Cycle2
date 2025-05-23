using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

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
        transform.DOMoveY(initialPosition, duration, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    public void TweenYUp()
    {
        transform.DOMoveY(initialPosition + 0.5f, duration, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }
}
