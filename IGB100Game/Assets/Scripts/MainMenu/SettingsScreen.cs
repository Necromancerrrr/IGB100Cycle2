using System;
using UnityEngine;

public class SettingsScreen : MonoBehaviour
{
    [SerializeField] GameObject settings;

    public void ShowSettings()
    {
        Debug.Log(settings.activeSelf);

        if (settings.activeSelf == true)
        {
            settings.SetActive(false);
        }

        else if (!settings.activeSelf)
        {
            settings.SetActive(true);
        }
    }
}
