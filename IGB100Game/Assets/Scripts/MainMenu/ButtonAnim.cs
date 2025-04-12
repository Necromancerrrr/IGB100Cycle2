using System.Collections.Generic;
using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text ButtonText;
    public MainMenu menuManager;
    private List<TMP_FontAsset> FontList;
    private List<int> FontSize;
    bool hovered = false;
    float timer = 0f; float animateCD = 0.15f;
    void Start()
    {
        FontList = menuManager.FontList;
        FontSize = menuManager.FontSize;
    }
    void Update()
    {
        AnimateMethod();
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }
    public void AnimateMethod()
    {
        if (timer <= 0f && hovered)
        {
            int RNG = Random.Range(0, FontList.Count);
            ButtonText.font = FontList[RNG];
            ButtonText.fontSize = FontSize[RNG];
            timer = animateCD;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
