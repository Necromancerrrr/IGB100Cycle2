using UnityEngine;
using DG.Tweening;

public class UpgradeCard : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private GameObject cardImage;
    //private RectTransform cardRectTransform;

    private UpgradeCardContainer cardContainer;

    private RectTransform m_RectTransform;
    public Vector2 initialPos;

    private bool clicked;

    private Tween aTween;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        initialPos = new Vector2(m_RectTransform.anchoredPosition.x, m_RectTransform.anchoredPosition.y);
        Debug.Log(initialPos);
        //DOAnchorPos
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
            DOTween.Kill(m_RectTransform);
            //aTween = null;
            m_RectTransform.DOLocalRotate(new Vector3(0, 0, 0), 0).SetUpdate(true);
            m_RectTransform.DOAnchorPos(initialPos, duration, false).SetEase(Ease.OutQuint).SetUpdate(true);
        }
    }

    public void TweenYUp()
    {
        if (cardContainer.clickable == true && clicked == false)
        {
            m_RectTransform.DOAnchorPos(new Vector2(initialPos.x, initialPos.y + 50f), duration, false).SetEase(Ease.OutQuint).SetUpdate(true);
            
            if (m_RectTransform.localRotation == Quaternion.Euler(0, 0, 0))
            {
                m_RectTransform.DOLocalRotate(new Vector3(0, 0, -0.5f), 0.2f).SetUpdate(true).OnComplete(() => 
                {
                    m_RectTransform.DOLocalRotate(new Vector3(0, 0, 0.5f), 0.2f).SetUpdate(true).SetLoops(-1, LoopType.Yoyo);
                });
                Debug.Log("AAA");
            }
        }
    }

    public void CardClicked()
    {
        clicked = true;
        m_RectTransform.DOAnchorPos(initialPos, duration, false).SetEase(Ease.OutQuint).SetUpdate(true).OnComplete(() =>
        {
            clicked = false;
            DOTween.Kill(m_RectTransform);
        });
    }
}
