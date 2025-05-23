using UnityEngine;
using UnityEngine.EventSystems;

public class CardBlink : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("CardBlink", 0, Random.Range(0.0f, 1.0f));
        anim.speed = 0;
    }
    public void AnimPlay()
    {
        anim.speed = Random.Range(0.7f, 1.3f);
    }
    public void AnimPause()
    {
        anim.speed = 0;
    }
}
