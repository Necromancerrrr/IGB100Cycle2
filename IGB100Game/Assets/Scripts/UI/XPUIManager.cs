using UnityEngine;
using UnityEngine.UI;

public class XPUIManager : MonoBehaviour
{
    public Slider slider;

    public void SetXPCap(int XPCap)
    {
        slider.maxValue = XPCap;
    }

    public void SetXPBar(int XP)
    {
        slider.value = XP;
    }
}
