using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private HelpOverlay helpMenu;
    public void SceneChange(string name)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(name);
    }
    public void Help()
    {
        helpMenu.ButtonPress();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
