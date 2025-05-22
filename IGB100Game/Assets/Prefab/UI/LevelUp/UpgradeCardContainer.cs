using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class UpgradeCardContainer : MonoBehaviour
{

    [SerializeField] private Transform firstCard;
    [SerializeField] private Transform secondCard;
    [SerializeField] private Transform thirdCard;

    [SerializeField] private Transform[] cards;

    private float duration = 0.5f;
    private float timer;
    private bool currentlyTweening = false;
    private int currentCardNum = 0;

    private GameManager gameManager;

    //Sequence initialSequence = DOTween.Sequence();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //initialSequence.Append(cards[0].DOScale(0f, duration).SetUpdate(true));

        //initialScale = transform.localScale;
        //transform.localScale = new Vector3(0, 0, 0);

        gameManager = FindFirstObjectByType<GameManager>();
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].localScale = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.currentState != GameManager.GameState.LevelUp)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].localScale = new Vector3(0, 0, 0);
            }
        }

        if (gameManager.currentState == GameManager.GameState.LevelUp && currentlyTweening == false)
        {
            InitialTween(currentCardNum);
            Debug.Log("AAAAA");
        }

        if (cards[currentCardNum].localScale.x > 0.5)
        {
            Debug.Log("AAAA");
            currentCardNum++;
            currentlyTweening = false;
        }
    }

    

    //TweenCallback
    //.OnComplete(InitialTween(currentCard))

    public TweenCallback InitialTween(int currentCard)
    {
        if (currentCard == cards.Length) return null;
        Debug.Log(currentCard);
        Transform currentCardTransform = cards[currentCard];

        currentlyTweening = true;

        currentCardTransform.DOScale(1f, duration).SetUpdate(true).SetEase(Ease.OutQuint);
        currentCardTransform.DOLocalRotate(new Vector3(0, 0, -360), duration, RotateMode.FastBeyond360).SetUpdate(true).SetEase(Ease.OutQuint).OnComplete(TweenCompleted);

        return null;
    }

    public void TweenCompleted()
    {
        Debug.Log("AAAA");
        //currentCardNum++;
        //currentlyTweening = false;
    }

    /*
    public void test(Transform card)
    {
        currentlyTweening = true;
        transform.DOScale(0f, duration);
        transform.DOLocalRotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360);
    }
    */
}
