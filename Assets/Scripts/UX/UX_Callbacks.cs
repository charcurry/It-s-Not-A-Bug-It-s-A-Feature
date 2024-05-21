using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UXCallbacks : MonoBehaviour
{
    // State management for keyboard keys
    private KeyState keyStates;

    // GameObject references for UI components
    public GameObject titleText;
    public GameObject crosshairImageGameObject;
    public GameObject canvasBackground;
    public GameObject mainMenu;
    public GameObject videoSettings;
    public GameObject controls;
    public GameObject inGameOverlay;
    public GameObject paused;
    public GameObject gameOver;

    public GameObject controlsBackToMenu;
    public GameObject controlsReturnToPaused;

    public GameObject settingsBackToMenu;
    public GameObject settingsReturnToPaused;

    private bool inGame = false;

    // Timer variables for UI changes
    private float startTime = 0.0f;

    // Enum for managing window modes
    public enum WindowMode : int
    {
        Fullscreen = 0,
        ExclusiveFullscreen,
        Windowed,
        Max,
    }

    // Enum for tracking the current UI state
    public enum UICurrentState
    {
        Menu = 0,
        MainMenu,
        VideoSettings,
        Controls,
        InGameOverlay,
        Paused,
        GameOver,
        Max,
    }

    public enum UXCallbacksNoParameter
    {
        Play = 0,
        Exit,
        Controls,
        VideoSettings,
        ReturnToMenu,
        ReturnToPausedState,
        PreviousScene,
        Max,
    }

    private bool titleUpdateFinished = false;

    void UpdateTitle()
    {
        if (titleUpdateFinished)
        {
            return;
        }

        if (UnityEngine.Random.value > 0.25f)
        {
            titleText.GetComponent<TMP_Text>().text = "ITS NOT A BUG ITS A FEATURE";
            titleUpdateFinished = true;
        }
        else
        {
            titleText.GetComponent<TMP_Text>().text = "           PUZZLE FACTORY";
        }
    }

    // Variables to hold current and previous UI states
    private UICurrentState uiState = UICurrentState.Menu;
    private UICurrentState prevUIState = UICurrentState.Menu;

    // Handles UI state changes and updates the visibility and functionality of UI elements accordingly
    void OnUIStateChange(UICurrentState newUIState)
    {
        inGame = false;
        UpdateTitle();
        prevUIState = uiState;
        uiState = newUIState;

        inGame = uiState == UICurrentState.InGameOverlay || newUIState == UICurrentState.Paused;

        // Toggle visibility of return buttons in the controls menu based on current state
        if (newUIState == UICurrentState.Controls && prevUIState == UICurrentState.Paused)
        {
            inGame = true;
            controlsReturnToPaused.SetActive(true);
            controlsBackToMenu.SetActive(false);
        }
        else
        {
            controlsReturnToPaused.SetActive(false);
            controlsBackToMenu.SetActive(true);
        }

        if (newUIState == UICurrentState.VideoSettings && prevUIState == UICurrentState.Paused)
        {
            inGame = true;
            settingsReturnToPaused.SetActive(true);
            settingsBackToMenu.SetActive(false);
        }
        else
        {
            settingsReturnToPaused.SetActive(false);
            settingsBackToMenu.SetActive(true);
        }

        gameOver.SetActive(uiState == UICurrentState.GameOver);
        // Manage visibility of various UI elements based on the current state
        titleText.SetActive(uiState != UICurrentState.InGameOverlay);
        canvasBackground.SetActive(uiState != UICurrentState.InGameOverlay);
        mainMenu.SetActive(uiState == UICurrentState.Menu);
        videoSettings.SetActive(uiState == UICurrentState.VideoSettings);
        controls.SetActive(uiState == UICurrentState.Controls);
        inGameOverlay.SetActive(uiState == UICurrentState.InGameOverlay);
        crosshairImageGameObject.SetActive(uiState == UICurrentState.InGameOverlay);
        paused.SetActive(uiState == UICurrentState.Paused);
    }

    // Handles changes in the window mode based on user selection
    public void OnWindowModeChange(int windowMode)
    {
        switch ((WindowMode)windowMode)
        {
            case WindowMode.Fullscreen:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case WindowMode.ExclusiveFullscreen:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case WindowMode.Windowed:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                break;
        }
    }

    // Handles button callbacks for various UI interactions
    public void OnUXCallback(int callback)
    {
        // Handling different callbacks based on the enum
        switch ((UXCallbacksNoParameter)callback)
        {
            case UXCallbacksNoParameter.Play:
                if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }

                OnUIStateChange(UICurrentState.InGameOverlay);
                break;
            case UXCallbacksNoParameter.Exit:
                Application.Quit();
                break;
            case UXCallbacksNoParameter.Controls:
                OnUIStateChange(UICurrentState.Controls);
                break;
            case UXCallbacksNoParameter.VideoSettings:
                OnUIStateChange(UICurrentState.VideoSettings);
                break;
            case UXCallbacksNoParameter.ReturnToMenu:
                SceneManager.LoadScene(0);
                OnUIStateChange(UICurrentState.Menu);
                break;
            case UXCallbacksNoParameter.ReturnToPausedState:
                OnUIStateChange(UICurrentState.Paused);
                break;
            case UXCallbacksNoParameter.PreviousScene:
                if (SceneManager.GetActiveScene().buildIndex - 1 == 0)
                {
                    SceneManager.LoadScene(0);
                    OnUIStateChange(UICurrentState.Menu);
                }
                else if (SceneManager.GetActiveScene().buildIndex - 1 > 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                    OnUIStateChange(UICurrentState.InGameOverlay);
                }
                break;
            default:
                break;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int lastSceneIndex = SceneManager.sceneCountInBuildSettings - 1;

        if (scene.buildIndex == lastSceneIndex)
        {
            OnUIStateChange(UICurrentState.GameOver);
        }
    }

    // Initialize KeyStates and start timer
    void Start()
    {
        keyStates = GetComponent<KeyState>();
        startTime = Time.time;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame to check key presses and update UI based on game state
    void Update()
    {
        // Check for ESC key to toggle pause state
        if (keyStates.CheckKeyState(KeyCode.Escape, EKeyQueryMode.KEYQUERY_SINGLEPRESS))
        {
            if (uiState == UICurrentState.InGameOverlay)
                OnUIStateChange(UICurrentState.Paused);
            else if (uiState == UICurrentState.Paused || inGame)
                OnUIStateChange(UICurrentState.InGameOverlay);
        }

        // Manage game time and cursor based on current UI state
        if (uiState == UICurrentState.InGameOverlay)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}