using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Slider slider;

    public void SetHealthMax(float healthCap)
    {
        slider.maxValue = healthCap;
    }

    public void SetHealthValue(float health)
    {
        slider.value = health;
    }
}
