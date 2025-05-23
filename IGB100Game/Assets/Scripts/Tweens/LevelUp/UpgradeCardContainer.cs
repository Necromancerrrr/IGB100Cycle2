using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class UpgradeCardContainer : MonoBehaviour
{
    [SerializeField] private Transform[] cards;

    private float duration = 0.5f;
    private bool currentlyTweening = false;
    private int currentCardNum = 0;

    private GameManager gameManager;

    private bool currentlyShrunk = true;

    public bool clickable = false;
    private float clickableTimer = 0;

    //Sequence initialSequence = DOTween.Sequence();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].localScale = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentCardNum);
        //gameObject.activeSelf
        

        if (gameManager.currentState == GameManager.GameState.LevelUp && currentlyTweening == false && currentCardNum < 3)
        {
            InitialTween(currentCardNum);
        }

        if (cards[currentCardNum].localScale.x > 0.5 && currentCardNum < 2)
        {
            currentCardNum++;
            currentlyTweening = false;
        }

        if (clickableTimer > 0.5)
        {
            clickable = true;
        }
        else
        {
            clickableTimer += Time.deltaTime;
        }
    }

    

    //TweenCallback
    //.OnComplete(InitialTween(currentCard))

    
    public void InitialTween(int currentCard)
    {
        Transform currentCardTransform = cards[currentCard];

        currentlyTweening = true;

        currentCardTransform.DOScale(1f, duration).SetUpdate(true).SetEase(Ease.OutQuint);
        currentCardTransform.DOLocalRotate(new Vector3(0, 0, -360), duration, RotateMode.FastBeyond360).SetUpdate(true).SetEase(Ease.OutQuint);
    }

    public void CleanUp() // makes everything ready for the next level up, triggers in GameManager & is called in EndLevelUp()
    {
        cards[0].localScale = new Vector3(0, 0, 0);
        cards[1].localScale = new Vector3(0, 0, 0);
        cards[2].localScale = new Vector3(0, 0, 0);
        cards[0].rotation = Quaternion.Euler(0, 0, 0);
        cards[1].rotation = Quaternion.Euler(0, 0, 0);
        cards[2].rotation = Quaternion.Euler(0, 0, 0);
        for (int i = 0; i < cards.Length; i++)
            {
                cards[i].localScale = new Vector3(0, 0, 0);
                cards[i].rotation = Quaternion.Euler(0, 0, 0);
            }
            currentCardNum = 0;
            currentlyTweening = false;
    }
}