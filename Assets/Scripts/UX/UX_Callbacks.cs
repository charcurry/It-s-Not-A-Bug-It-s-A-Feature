using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class UX_Callbacks : MonoBehaviour
{

    private KeyState KeyStates;
    public GameObject CanvasBackground;
    public GameObject MainMenu;
    public GameObject VideoSettings;
    public GameObject Controls;
    public GameObject InGameOverlay;
    public GameObject Paused;

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
        BUTTON_CALLBACK_MAX,
    }

    private EUICurrentState UIState = EUICurrentState.UI_STATE_MENU;
    private EUICurrentState PrevUIState = EUICurrentState.UI_STATE_MENU;
    void OnUIStateChange(EUICurrentState _UIState)
    {
        PrevUIState = UIState;
        UIState = _UIState;

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
                    OnUIStateChange(EUICurrentState.UI_STATE_MENU);
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

    // Update is called once per frame
    void Update()
    {
        if (KeyStates.CheckKeyState(KeyCode.Escape,EKeyQueryMode.KEYQUERY_SINGLEPRESS))
        {
            if (UIState == EUICurrentState.UI_STATE_INGAME_OVERLAY)
                OnUIStateChange(EUICurrentState.UI_STATE_PAUSED);
            else if (UIState == EUICurrentState.UI_STATE_PAUSED)
                OnUIStateChange(EUICurrentState.UI_STATE_INGAME_OVERLAY);
        }
    }
}
