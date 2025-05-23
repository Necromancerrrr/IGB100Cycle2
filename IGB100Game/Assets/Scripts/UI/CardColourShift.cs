//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class CardColourShift : MonoBehaviour
{
    [SerializeField] Image AreaSize;
    [SerializeField] Image ProjectileCount;
    [SerializeField] Image Duration;
    [SerializeField] Sprite weaponCard;
    [SerializeField] Sprite statCard;
    Image cardImage;
    Animator anim;

    void Start()
    {
        cardImage = GetComponent<Image>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!AreaSize.enabled && !ProjectileCount.enabled && !Duration.enabled)
        {
            cardImage.sprite = statCard;
            anim.SetBool("CardColour", false);
        }
        else
        {
            cardImage.sprite = weaponCard;
            anim.SetBool("CardColour", true);
        }
    }
}
