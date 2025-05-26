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
        transform.DOMoveY(initialPosition + 0.5f, duration, false).SetEase(Ease.OutQuint).SetUpdate(true);

        if (m_RectTransform.localRotation == Quaternion.Euler(0, 0, 0))
        {
            m_RectTransform.DOLocalRotate(new Vector3(0, 0, -0.5f), 0.2f).SetUpdate(true).OnComplete(() => 
            {
                m_RectTransform.DOLocalRotate(new Vector3(0, 0, 0.5f), 0.2f).SetUpdate(true).SetLoops(-1, LoopType.Yoyo);
            });
        }
    }

    public void TweenYDown()
    {
        DOTween.Kill(m_RectTransform);
        m_RectTransform.DOLocalRotate(new Vector3(0, 0, 0), 0).SetUpdate(true);
        transform.DOMoveY(initialPosition, duration, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }

}
