using System.Collections.Generic;
using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnim : MonoBehaviour
{
    public TMP_Text ButtonText;
    public MainMenu menuManager;
    private List<TMP_FontAsset> FontList;
    private List<int> FontSize;
    bool hovered = false;
    float timer = 0f; float animateCD = 0.15f; float cooldownVariance;


    //Pulls list of fonts and font size from MainMenu object
    void Start()
    {
        FontList = menuManager.FontList;
        FontSize = menuManager.FontSize;
    }
    void Update()
    {
        // AnimateMethod();
    }

    // If the button is hovered and the timer has elapsed, change the font randomly.
    // Otherwise, tick down the timer
    /*
    public void AnimateMethod()
    {
        if (timer <= 0f && hovered)
        {
            int RNG = Random.Range(0, FontList.Count);
            ButtonText.font = FontList[RNG];
            ButtonText.fontSize = FontSize[RNG];
            timer = animateCD + Random.Range(cooldownVariance * -1, cooldownVariance);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
    */
}
