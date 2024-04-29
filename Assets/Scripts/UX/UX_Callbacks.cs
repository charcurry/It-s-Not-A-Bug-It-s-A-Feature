using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UX_Callbacks : MonoBehaviour
{
    // Start is called before the first frame update
  
    public GameObject CanvasBackground;
    public GameObject MainMenu;
    public GameObject OptionsScreen;
    public GameObject IngameOverlay;
    public GameObject PausedOverlay;

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
        UI_STATE_PAUSED,
        UI_STATE_OPTIONS,
        UI_STATE_INGAME,
        UI_STATE_MAX,
    }
    public enum EUXCallbacksNoParameter
    {
        BUTTON_CALLBACK_PLAY =0 ,
        BUTTON_CALLBACK_EXIT,
        BUTTON_CALLBACK_OPTIONS,
        BUTTON_CALLBACK_RETURN_TO_MENU,
    }

    private EUICurrentState UIState = EUICurrentState.UI_STATE_MENU;
    private EUICurrentState PrevUIState = EUICurrentState.UI_STATE_MENU;
    void OnUIStateChange(EUICurrentState _UIState)
    {
        PrevUIState = UIState;
        UIState = _UIState;

        CanvasBackground.SetActive(UIState != EUICurrentState.UI_STATE_INGAME);
        MainMenu.SetActive(UIState == EUICurrentState.UI_STATE_MENU);
        OptionsScreen.SetActive(UIState == EUICurrentState.UI_STATE_OPTIONS);
        IngameOverlay.SetActive(UIState == EUICurrentState.UI_STATE_INGAME);
        PausedOverlay.SetActive(UIState == EUICurrentState.UI_STATE_PAUSED);
    }

    public void OnWindowModeChange(Int32 WindowMode)
    {
        switch ((EWindowMode)WindowMode)
        {
            case EWindowMode.WND_MODE_FULLSCREEN:
                {
                  //  Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                  //  Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                    break;
                }
            case EWindowMode.WND_MODE_EXCLUSIVE_FULLSCREEN:
                {
                    //Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                }
            case EWindowMode.WND_MODE_WINDOWED:
                {
                    //Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                }
            default:
                break;
        }
  
    }
    public void OnUXCallback(EUXCallbacksNoParameter eCallback)
    {
        switch (eCallback)
        {
            case EUXCallbacksNoParameter.BUTTON_CALLBACK_PLAY:
                {

                    break;
                }
            case EUXCallbacksNoParameter.BUTTON_CALLBACK_EXIT:
                {

                    break;
                }
            case EUXCallbacksNoParameter.BUTTON_CALLBACK_OPTIONS:
                {

                    break;
                }
            case EUXCallbacksNoParameter.BUTTON_CALLBACK_RETURN_TO_MENU:
                {

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
