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

    private AudioManager audioManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        initialPos = new Vector2(m_RectTransform.anchoredPosition.x, m_RectTransform.anchoredPosition.y);
        Debug.Log(initialPos);
        //DOAnchorPos
        cardContainer = FindFirstObjectByType<UpgradeCardContainer>();

        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();

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
            m_RectTransform.DOLocalRotate(new Vector3(0, 0, 0), 0).SetUpdate(true);
            m_RectTransform.DOAnchorPos(initialPos, duration, false).SetEase(Ease.OutCubic).SetUpdate(true);
        }
    }

    public void TweenYUp()
    {
        if (cardContainer.clickable == true && clicked == false)
        {
            audioManager.PlaySFX(audioManager.LevelUpCardHover, m_RectTransform, 1f);

            m_RectTransform.DOAnchorPos(new Vector2(initialPos.x, initialPos.y + 50f), duration, false).SetEase(Ease.OutCubic).SetUpdate(true);

            m_RectTransform.DOLocalRotate(new Vector3(0, 0, -0.5f), 0.3f).SetUpdate(true).OnComplete(() =>
            {
                m_RectTransform.DOLocalRotate(new Vector3(0, 0, 0.5f), 0.3f).SetUpdate(true).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            });
            /*
            if (m_RectTransform.localRotation == Quaternion.Euler(0, 0, 0))
            {
                
            }
            */
        }
    }

    public void CardClicked()
    {
        audioManager.PlaySFX(audioManager.LevelUpEnd, m_RectTransform, 1f);

        clicked = true;
        m_RectTransform.DOAnchorPos(initialPos, duration, false).SetEase(Ease.OutQuint).SetUpdate(true).OnComplete(() =>
        {
            clicked = false;
            DOTween.Kill(m_RectTransform);
        });


    }
}
