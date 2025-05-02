using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public List<TMP_FontAsset> FontList;
    [SerializeField] public List<int> FontSize;
    /*
    // Logic for buttons
    public void CharacterSelect()
    {
        SceneManager.LoadScene("CharacterSelectScreen");
    }

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }
    */
}
