using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Flag to check if the game is over
    public bool isGameOver = false;

    //Define the different states of the game
    public enum GameState
    {
        Gameplay,
        Pause,
        GameOver
    }

    // Store the current state of the game
    public GameState currentState;
    // Store the previous state of the game
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;


    // Current stat displays
    [Header("Current Stat Displays")]
    public TMP_Text currentHealthDisplay;
    public TMP_Text currentRecoveryDisplay;
    public TMP_Text currentMoveSpeedDisplay;
    public TMP_Text currentAttackDisplay;
    public TMP_Text currentProjectileSpeedDisplay;
    public TMP_Text currentMagnetDisplay;

    
    [Header("Results Screen Displays")]
    public Image chosenCharacterImage;
    public TMP_Text chosenCharacterName;

    public TMP_Text levelReachedDisplay;

    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);

    void Awake()
    {
        // Singleton check
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EXTRA" + this + " DELETED");
            Destroy(gameObject);
        }

        DisableScreens();
    }


    void Update()
    {
        // Define the behaviour for each state
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                break;

            case GameState.Pause:
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f; // Stop the game
                    Debug.Log("GAME IS OVER");
                    DisplayResults();
                }
                break;
            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        }
    }

    //Define the method to change the state of the game
    public void ChangeGameState(GameState newState)
    {
        currentState = newState;
    }

    // Method to pause the game
    public void PauseGame()
    {
        if (currentState != GameState.Pause)
        {
            previousState = currentState;       // Stores the state the player was in before pausing
            ChangeGameState(GameState.Pause);   // Puts the player in the "Paused" state
            Time.timeScale = 0f;                // Stops the game
            pauseScreen.SetActive(true);
            Debug.Log("Game is paused");
        }
    }


    // Method to resume the game 
    public void ResumeGame()
    {
        if (currentState == GameState.Pause)
        {
            ChangeGameState(previousState); // Returns back into the state the player was last in before pausing
            Time.timeScale = 1.0f;          // Resume the game
            pauseScreen.SetActive(false);
            Debug.Log("Game is resumed");
        }
    }

    void CheckForPauseAndResume()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (currentState == GameState.Pause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
    }

    public void GameOver()
    {
        ChangeGameState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.Name;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenWeaponsAndPassiveItemsUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        if (chosenWeaponsData.Count != chosenWeaponsUI.Count || chosenPassiveItemsData.Count != chosenPassiveItemsUI.Count) 
        {
            Debug.Log("Chosen weapons and passive items lists have different lengths");
            return;
        }

        // Assign chosen weapons data to the chosenWeaponsUI
        for (int i = 0; i < chosenWeaponsUI.Count; i++) 
        {
            // Check that the sprite of the corresponding element in chosenWeaponsData is not null
            if (chosenWeaponsData[i].sprite)
            {
                // Enable the corresponding element in the chosenWepaonsUI and set its sprite to the corresponding sprite in chosenWeaponsData
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;
            }
            else
            {
                // If the sprite is null, disable the element instead
                chosenWeaponsUI[i].enabled = false;
            }
        }

        // Assign chosen weapons data to the chosenPassiveItemsUI
        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            // Check that the sprite of the corresponding element in chosenPassiveItemsData is not null
            if (chosenPassiveItemsData[i].sprite)
            {
                // Enable the corresponding element in the chosenPassiveItemsUI and set its sprite to the corresponding sprite in chosenPassiveItemsData
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else
            {
                // If the sprite is null, disable the element instead
                chosenPassiveItemsUI[i].enabled = false;
            }
        }
    }
}
