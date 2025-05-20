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

    // Flag to check if the player is levelling up
    public bool isChoosingLvlUp = false;

    // Flag to check if the player is levelling up
    public bool IsChoosingPact = false;

    // Reference to the player
    public GameObject playerObject;

    //Define the different states of the game
    public enum GameState
    {
        Gameplay,
        Pause,
        GameOver,
        LevelUp,
        PactChoice,
    }

    // Store the current state of the game
    public GameState currentState;
    // Store the previous state of the game
    public GameState previousState;

    [Header("Damage Text Settings")]
    public Canvas damageTextCanvas;
    //public float textFontSize;
    public TMP_FontAsset textFont;
    public Camera referenceCamera;

    [Header("Dev Buttons")]
    public GameObject devButtons;

    [Header("Stopwatch")]
    public float timeLimit; // Measured in seconds
    float stopwatchTime;
    public TMP_Text stopwatchDisplay;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;
    public GameObject pactSelectScreen;


    // Current stat displays
    [Header("Current Stat Displays")]
    public TMP_Text currentHealthDisplay;
    public TMP_Text currentRecoveryDisplay;
    public TMP_Text currentMoveSpeedDisplay;
    public TMP_Text currentAttackDisplay;
    public TMP_Text currentProjectileSpeedDisplay;
    public TMP_Text currentProjectileDurationDisplay;
    public TMP_Text currentProjectileCountDisplay;
    public TMP_Text currentAOEDisplay;
    public TMP_Text currentMagnetDisplay;
    public TMP_Text currentKillsDisplay;

    
    [Header("Results Screen Displays")]
    public Image chosenCharacterImage;
    public TMP_Text chosenCharacterName;

    public TMP_Text levelReachedDisplay;

    public TMP_Text timeSurvivedDisplay;

    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);
    public List<Image> chosenPactsUI = new List<Image>(2);

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
                CheckDevMode();
                UpdateStopwatch();
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

            case GameState.LevelUp:
                if (!isChoosingLvlUp)
                {
                    isChoosingLvlUp = true;
                    Time.timeScale = 0f; // Stops the game\
                    Debug.Log("Player is levelling up and choosing weapon");
                    levelUpScreen.SetActive(true);
                }
                break;

            case GameState.PactChoice:
                if (!IsChoosingPact)
                {
                    IsChoosingPact = true;
                    Time.timeScale = 0f;
                    Debug.Log("Player is CHOOSING PACT");
                    pactSelectScreen.SetActive(true);
                }
                break;

            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        }
    }

    IEnumerator GenerateFloatingTextCoroutine(float number, Transform target, Color colour, float size, float duration = 1f, float speed = 50f)
    {
        GameObject textObj = new GameObject("Damage Floating Text");
        RectTransform rect = textObj.AddComponent<RectTransform>();
        TextMeshProUGUI tmPro = textObj.AddComponent<TextMeshProUGUI>();

        tmPro.text = Mathf.FloorToInt(number).ToString();
        tmPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
        tmPro.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmPro.color = colour;
        if (number >= 10) { tmPro.fontSize = size + (number - 10); } // If the number is larger than 10, make it large
        else { tmPro.fontSize = size; }
        

        if (textFont)
        {
            tmPro.font = textFont;
        }

        rect.position = target.position;
            //referenceCamera.WorldToScreenPoint(target.position);

        Destroy(textObj, duration);

        textObj.transform.SetParent(instance.damageTextCanvas.transform);
        textObj.transform.SetAsFirstSibling();

        textObj.transform.localScale = Vector3.one;

        Vector3 initialSpawn = target.position;

        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        float yOffset = 0;
        while (t < duration) 
        {
            tmPro.color = new Color(tmPro.color.r, tmPro.color.g, tmPro.color.b, 1 - t / duration);
            if (target)
            {
                yOffset += speed * Time.deltaTime;
                rect.position = initialSpawn + new Vector3(0, yOffset);
                    //referenceCamera.WorldToScreenPoint(target.position + new Vector3(0, yOffset));
            }
            else
            {
                rect.position += new Vector3(0, speed * Time.deltaTime, 0);
            }

            yield return w;
            t += Time.deltaTime;
        }
    }

    public static void GenerateFloatingText(float number, Transform target, Color colour, float size, float duration = 1f, float speed = 1f)
    {
        // If the canvas is not set, end the function so we don't generate floating text
        if (!instance.damageTextCanvas) { return; }

        // Find a relevant camera that can be used to convert the world position to a screen position
        if (!instance.referenceCamera) { instance.referenceCamera = Camera.main; }

        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(number,target,colour,size,duration,speed));   
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

    void CheckDevMode()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            if (devButtons.activeSelf)
            {
                devButtons.SetActive(false);
            }
            else 
            {
                devButtons.SetActive(true);
            }
        }
    }

    public void ResetLevelState()
    {
        Time.timeScale = 1.0f;
    }

    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if (stopwatchTime >= timeLimit)
        {
            GameOver();
        }
    }

    void UpdateStopwatchDisplay()
    {
        // Calculate the number of minutes and seconds that have elapsed
        int minutes = Mathf.FloorToInt(stopwatchTime / 60); // Divison = Minutes
        int seconds = Mathf.FloorToInt(stopwatchTime % 60); // Modulus = Seconds

        // Update the stopwatch text to display the timer
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
        pactSelectScreen.SetActive(false);
        devButtons.SetActive(false);
    }

    public void GameOver()
    {
        ChangeGameState(GameState.GameOver);
    }

    void DisplayResults()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
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

    public void StartLevelUp()
    {
        ChangeGameState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        isChoosingLvlUp = false;
        Time.timeScale = 1.0f; // Resumes game
        levelUpScreen.SetActive(false);
        ChangeGameState(GameState.Gameplay);
    }

    public void StartPactChoice()
    {
        ChangeGameState(GameState.PactChoice);
        playerObject.SendMessage("RemoveAndApplyPacts");
    }

    public void EndPactChoice()
    {
        IsChoosingPact = false;
        Time.timeScale = 1.0f; // Resumes game
        pactSelectScreen.SetActive(false);
        ChangeGameState(GameState.Gameplay);
    }
}
