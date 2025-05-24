using UnityEngine;
using DG.Tweening;
//using Unity.VisualScripting;

public class UpgradeCardContainer : MonoBehaviour
{
    [SerializeField] private GameObject[] cards;

    private float duration = 0.5f;
    private bool currentlyTweening = false;
    private int currentCardNum = 0;

    private GameManager gameManager;

    public bool clickable = false;
    private float clickableTimer = 0;

    //Sequence initialSequence = DOTween.Sequence();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].transform.localScale = new Vector3(0, 0, 0);
            cards[i].transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.activeSelf

        if (gameManager.currentState == GameManager.GameState.LevelUp && currentlyTweening == false && currentCardNum < 3)
        {
            InitialTween(currentCardNum);
        }

        if (cards[currentCardNum].transform.localScale.x > 0.2f && currentCardNum < 2)
        {
            currentCardNum++;
            currentlyTweening = false;
        }

        if (clickableTimer > 0.7f)
        {
            clickable = true;
        }
        else
        {
            clickableTimer += Time.unscaledDeltaTime;
        }
    }



    public void InitialTween(int currentCard)
    {
        Transform currentCardTransform = cards[currentCard].transform;

        currentlyTweening = true;

        Sequence scaleSequence = DOTween.Sequence();

        scaleSequence.Append(currentCardTransform.DOScale(1.5f, duration/2f).SetUpdate(true).SetEase(Ease.InCubic));
        scaleSequence.Append(currentCardTransform.DOScale(1f, duration/2f).SetUpdate(true).SetEase(Ease.OutCubic));
        scaleSequence.Play().SetUpdate(true);

        currentCardTransform.DOLocalRotate(new Vector3(0, 0, -720), duration/1.5f, RotateMode.FastBeyond360).SetUpdate(true).SetEase(Ease.OutCubic);
    }

    public void CleanUp() // makes everything ready for the next level up, triggers in GameManager & is called in EndLevelUp()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].transform.DOScale(0, 0).SetUpdate(true);
            cards[i].transform.DOLocalRotate(new Vector3(0, 0, 0), 0).SetUpdate(true);
        }

        currentCardNum = 0;
        currentlyTweening = false;
        clickableTimer = 0;
        clickable = false;
    }
}