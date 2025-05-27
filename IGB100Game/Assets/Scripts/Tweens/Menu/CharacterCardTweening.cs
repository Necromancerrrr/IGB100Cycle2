using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class CharacterCardTweening : MonoBehaviour
{
    private RectTransform m_RectTransform;

    private float initialPosition;

    [SerializeField] private float duration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();

        initialPosition = transform.position.y;
    }

    public void TweenYUp()
    {
        transform.DOMoveY(initialPosition + 0.5f, duration, false).SetEase(Ease.OutCubic).SetUpdate(true);

        m_RectTransform.DOLocalRotate(new Vector3(0, 0, -0.5f), 0.3f).SetUpdate(true).OnComplete(() =>
        {
            m_RectTransform.DOLocalRotate(new Vector3(0, 0, 0.5f), 0.3f).SetUpdate(true).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        });
    }

    public void TweenYDown()
    {
        DOTween.Kill(m_RectTransform);
        m_RectTransform.DOLocalRotate(new Vector3(0, 0, 0), 0).SetUpdate(true);
        transform.DOMoveY(initialPosition, duration, false).SetEase(Ease.OutCubic).SetUpdate(true);
    }

    public void TweenClicked()
    {
        m_RectTransform.DOScale(1.2f, 0.1f).SetUpdate(true).SetEase(Ease.OutElastic);
        /*
        m_RectTransform.DOScale(0.8f, 0.05f).SetUpdate(true).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            
        });
        */
    }
}

