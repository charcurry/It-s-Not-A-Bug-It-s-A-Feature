using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UX_Callbacks : MonoBehaviour
{

    private KeyState KeyStates;
    public GameObject CanvasBackground;
    public GameObject MainMenu;
    public GameObject VideoSettings;
    public GameObject Controls;
    public GameObject InGameOverlay;
    public GameObject Paused;

    public GameObject Controls_BackToMenu;
    public GameObject Controls_ReturnToPaused;

    //Interaction UI
    //Notification handler
    //Sound settings
    //Objective UI
    //Narrator UI
    //Add crosshairs
    public enum EWindowMode : Int32
    {
        WND_MODE_FULLSCREEN = 0,
        WND_MODE_EXCLUSIVE_FULLSCREEN,
        WND_MODE_WINDOWED,
        WND_MODE_MAX,
    }
    public enum EUICurrentState
    {
        UI_STATE_MENU = 0,
        UI_STATE_MAIN_MENU,
        UI_STATE_VIDEO_SETTINGS,
        UI_STATE_CONTROLS,
        UI_STATE_INGAME_OVERLAY,
        UI_STATE_PAUSED,
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

    private EUICurrentState UIState = EUICurrentState.UI_STATE_MENU;
    private EUICurrentState PrevUIState = EUICurrentState.UI_STATE_MENU;
    void OnUIStateChange(EUICurrentState _UIState)
    {
        PrevUIState = UIState;
        UIState = _UIState;

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

        CanvasBackground.SetActive(UIState != EUICurrentState.UI_STATE_INGAME_OVERLAY);
        MainMenu.SetActive(UIState == EUICurrentState.UI_STATE_MENU);
        VideoSettings.SetActive(UIState == EUICurrentState.UI_STATE_VIDEO_SETTINGS);
        Controls.SetActive(UIState == EUICurrentState.UI_STATE_CONTROLS);
        InGameOverlay.SetActive(UIState == EUICurrentState.UI_STATE_INGAME_OVERLAY);
        Paused.SetActive(UIState == EUICurrentState.UI_STATE_PAUSED);
    }

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
    public void OnUXCallback(int eCallback)
    {
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

    void Start()
    {
        KeyStates = GetComponent<KeyState>();
        
    }
    void Update()
    {
        if (KeyStates.CheckKeyState(KeyCode.Escape,EKeyQueryMode.KEYQUERY_SINGLEPRESS))
        {
            if (UIState == EUICurrentState.UI_STATE_INGAME_OVERLAY)
                OnUIStateChange(EUICurrentState.UI_STATE_PAUSED);
            else if (UIState == EUICurrentState.UI_STATE_PAUSED)
                OnUIStateChange(EUICurrentState.UI_STATE_INGAME_OVERLAY);
        }

        if (UIState == EUICurrentState.UI_STATE_INGAME_OVERLAY)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (UIState == EUICurrentState.UI_STATE_PAUSED)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
}
