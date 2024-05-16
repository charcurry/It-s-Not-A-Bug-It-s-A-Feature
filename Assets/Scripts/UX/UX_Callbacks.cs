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

public class UX_Callbacks : MonoBehaviour
{

    // State management for keyboard keys
    private KeyState KeyStates;

    // GameObject references for UI components
    public GameObject TitleText;
    public GameObject CrosshairImageGameObject;
    public GameObject CanvasBackground;
    public GameObject MainMenu;
    public GameObject VideoSettings;
    public GameObject Controls;
    public GameObject InGameOverlay;
    public GameObject Paused;
    public GameObject GameOver;

    public GameObject Controls_BackToMenu;
    public GameObject Controls_ReturnToPaused;

    // Timer variables for UI changes
    private float flStartTime = 0.0f;

    // Enum for managing window modes
    public enum EWindowMode : Int32
    {
        WND_MODE_FULLSCREEN = 0,
        WND_MODE_EXCLUSIVE_FULLSCREEN,
        WND_MODE_WINDOWED,
        WND_MODE_MAX,
    }   
    
    // Enum for tracking the current UI state
    public enum EUICurrentState
    {
        UI_STATE_MENU = 0,
        UI_STATE_MAIN_MENU,
        UI_STATE_VIDEO_SETTINGS,
        UI_STATE_CONTROLS,
        UI_STATE_INGAME_OVERLAY,
        UI_STATE_PAUSED,
        UI_STATE_GAMEOVER,
        UI_STATE_MAX,
    }
    public enum EUXCallbacksNoParameter
    {
        BUTTON_CALLBACK_PLAY = 0,
        BUTTON_CALLBACK_EXIT,
        BUTTON_CALLBACK_CONTROLS,
        BUTTON_CALLBACK_VIDEO_SETTINGS,
        BUTTON_CALLBACK_RETURN_TO_MENU,
        BUTTON_CALLBACK_RETURN_TO_PAUSED_STATE,
        BUTTON_CALLBACK_PREVIOUS_SCENE,
        BUTTON_CALLBACK_MAX,
    }

    void UpdateTitle()
    {
        if (UnityEngine.Random.value > 0.25f)
        {
            TitleText.GetComponent<TMP_Text>().text = "ITS NOT A BUG ITS A FEATURE";
        }
        else
        {
            TitleText.GetComponent<TMP_Text>().text = "           PUZZLE FACTORY";
        }
    }

    // Variables to hold current and previous UI states
    private EUICurrentState UIState = EUICurrentState.UI_STATE_MENU;
    private EUICurrentState PrevUIState = EUICurrentState.UI_STATE_MENU;

    // Handles UI state changes and updates the visibility and functionality of UI elements accordingly
    void OnUIStateChange(EUICurrentState _UIState)
    {
        UpdateTitle();
        PrevUIState = UIState;
        UIState = _UIState;

        // Toggle visibility of return buttons in the controls menu based on current state
        if (_UIState == EUICurrentState.UI_STATE_CONTROLS && PrevUIState == EUICurrentState.UI_STATE_PAUSED)
        {
            Controls_ReturnToPaused.SetActive(true);
            Controls_BackToMenu.SetActive(false);
        }
        else
        {
            Controls_ReturnToPaused.SetActive(false);
            Controls_BackToMenu.SetActive(true);
        }

        GameOver.SetActive(UIState == EUICurrentState.UI_STATE_GAMEOVER);
        // Manage visibility of various UI elements based on the current state
        TitleText.SetActive(UIState != EUICurrentState.UI_STATE_INGAME_OVERLAY);
        CanvasBackground.SetActive(UIState != EUICurrentState.UI_STATE_INGAME_OVERLAY);
        MainMenu.SetActive(UIState == EUICurrentState.UI_STATE_MENU);
        VideoSettings.SetActive(UIState == EUICurrentState.UI_STATE_VIDEO_SETTINGS);
        Controls.SetActive(UIState == EUICurrentState.UI_STATE_CONTROLS);
        InGameOverlay.SetActive(UIState == EUICurrentState.UI_STATE_INGAME_OVERLAY);
        CrosshairImageGameObject.SetActive(UIState == EUICurrentState.UI_STATE_INGAME_OVERLAY);
        Paused.SetActive(UIState == EUICurrentState.UI_STATE_PAUSED);
    }


    // Handles changes in the window mode based on user selection
    public void OnWindowModeChange(Int32 WindowMode)
    {

        switch ((EWindowMode)WindowMode)
        {
            case EWindowMode.WND_MODE_FULLSCREEN:
                {
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
                }
            case EWindowMode.WND_MODE_EXCLUSIVE_FULLSCREEN:
                {
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                }
            case EWindowMode.WND_MODE_WINDOWED:
                {
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                }
            default:
                break;
        }

    }

    // Handles button callbacks for various UI interactions

    public void OnUXCallback(int eCallback)
    {


        // Handling different callbacks based on the enum
        switch ((EUXCallbacksNoParameter)eCallback)
        {
            case EUXCallbacksNoParameter.BUTTON_CALLBACK_PLAY:
                {

                    if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }

                    OnUIStateChange(EUICurrentState.UI_STATE_INGAME_OVERLAY);
                    break;
                }
            case EUXCallbacksNoParameter.BUTTON_CALLBACK_EXIT:
                {
                    Application.Quit();
                    break;
                }
            case EUXCallbacksNoParameter.BUTTON_CALLBACK_CONTROLS:
                {
                    OnUIStateChange(EUICurrentState.UI_STATE_CONTROLS);
                    break;
                }
            case EUXCallbacksNoParameter.BUTTON_CALLBACK_VIDEO_SETTINGS:
                {
                    OnUIStateChange(EUICurrentState.UI_STATE_VIDEO_SETTINGS);
                    break;
                }
            case EUXCallbacksNoParameter.BUTTON_CALLBACK_RETURN_TO_MENU:
                {
                    SceneManager.LoadScene(0);
                    OnUIStateChange(EUICurrentState.UI_STATE_MENU);
                    break;
                }
            case EUXCallbacksNoParameter.BUTTON_CALLBACK_RETURN_TO_PAUSED_STATE:
                {
                    OnUIStateChange(EUICurrentState.UI_STATE_PAUSED);
                    break;
                }
            case EUXCallbacksNoParameter.BUTTON_CALLBACK_PREVIOUS_SCENE:
                {
                    if (SceneManager.GetActiveScene().buildIndex - 1 == 0)
                    {
                        SceneManager.LoadScene(0);
                        OnUIStateChange(EUICurrentState.UI_STATE_MENU);
                    }
                    else if (SceneManager.GetActiveScene().buildIndex - 1 > 0)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                        OnUIStateChange(EUICurrentState.UI_STATE_INGAME_OVERLAY);
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int lastSceneIndex = SceneManager.sceneCountInBuildSettings - 1;

        if (scene.buildIndex == lastSceneIndex)
        {
            OnUIStateChange(EUICurrentState.UI_STATE_GAMEOVER);
        }

    }
    // Initialize KeyStates and start timer
    void Start()
    {
        KeyStates = GetComponent<KeyState>();
        flStartTime = Time.time;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame to check key presses and update UI based on game state
    void Update()
    {
        // Check for ESC key to toggle pause state

        if (KeyStates.CheckKeyState(KeyCode.Escape, EKeyQueryMode.KEYQUERY_SINGLEPRESS))
        {
            if (UIState == EUICurrentState.UI_STATE_INGAME_OVERLAY)
                OnUIStateChange(EUICurrentState.UI_STATE_PAUSED);
            else if (UIState == EUICurrentState.UI_STATE_PAUSED)
                OnUIStateChange(EUICurrentState.UI_STATE_INGAME_OVERLAY);
        }
        // Manage game time and cursor based on current UI state
        if (UIState == EUICurrentState.UI_STATE_INGAME_OVERLAY)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else /*if (UIState == EUICurrentState.UI_STATE_PAUSED)*/
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
