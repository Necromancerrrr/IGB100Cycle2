using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBlink2 : MonoBehaviour
{
    [SerializeField] Animator anim;
    void Start()
    {
        Set();
    }
    public void Set()
    {
        if (anim.GetBool("CardColour")) { anim.Play("CardBlink", 0, UnityEngine.Random.Range(0.0f, 1.0f)); }
        else { anim.Play("CardBlink2", 0, UnityEngine.Random.Range(0.0f, 1.0f)); }
        anim.speed = 0;
    }
    public void AnimPlay()
    {
        anim.speed = UnityEngine.Random.Range(0.7f, 1.3f);
    }
    public void AnimPause()
    {
        anim.speed = 0;
    }
}
