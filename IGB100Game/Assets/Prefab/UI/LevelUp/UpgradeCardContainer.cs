using UnityEngine;
using DG.Tweening;

public class UpgradeCardContainer : MonoBehaviour
{

    [SerializeField] private Transform firstCard;
    [SerializeField] private Transform secondCard;
    [SerializeField] private Transform thirdCard;

    [SerializeField] private Transform[] cards;

    private float duration = 1f;
    private float timer;
    private bool currentlyTweening = false;
    private int currentCardNum = 0;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //initialScale = transform.localScale;
        //transform.localScale = new Vector3(0, 0, 0);

        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;
        /*
        if (gameObject.activeSelf && currentlyTweening == false)
        {
            test(firstCard);
        }
        */

        if (gameManager.currentState == GameManager.GameState.LevelUp && currentlyTweening == false)
        {
            InitialTween(0);
        }
        //Debug.Log(firstCard.rotation);
    }



    
    public TweenCallback InitialTween(int currentCard)
    {
        if (currentCard == cards.Length + 1) return null;

        Transform currentCardTransform = cards[currentCard];

        currentlyTweening = true;
        currentCardTransform.DOScale(0f, duration).SetUpdate(true);
        currentCardTransform.DOLocalRotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360).SetUpdate(true).OnComplete(InitialTween(currentCard+1));
        

        return null;
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
