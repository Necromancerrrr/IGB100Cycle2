using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UIElements;
//using UnityEditor.SearchService;

public class SceneController : MonoBehaviour
{
    [SerializeField] private HelpOverlay helpMenu;

    public Animator animator;
    private string sceneName;

    private string currentSceneName;
    private float inputDelay = 0.5f;
    private float inputTimer;

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        currentSceneName = currentScene.name;
    }

    private void Update()
    {
        inputTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Player is pressing the Escape key");
            if (currentSceneName != null && inputTimer <=  0)
            {
                inputTimer = inputDelay;
                if (currentSceneName == "MainMenu" && !helpMenu.isActiveAndEnabled)
                {
                    Debug.Log("Player is attempting to quit the main menu");
                    Quit();
                }
                else if(currentSceneName == "CharacterSelectScreen")
                {
                    Debug.Log("Player is attempting to quit the Character Select Screen");
                    Back();
                }
            }
        }
    }

    public void FadeOutAnim(string name)
    {
        sceneName = name;
        currentSceneName = name;
        animator.SetTrigger("FadeOut");

    }

    public void Back()
    {
        if (CharacterSelector.instance != null)
        {
            CharacterSelector.instance.DestorySingleton();
        }
        FadeOutAnim("MainMenu");
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
