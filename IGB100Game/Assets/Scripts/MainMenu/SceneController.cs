using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private HelpOverlay helpMenu;

    public Animator animator;

    private string sceneName;

    public void FadeOutAnim(string name)
    {
        sceneName = name;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneName);
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
