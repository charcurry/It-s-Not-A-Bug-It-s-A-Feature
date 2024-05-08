using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Shared_UXVariables;

public class Shared_UXVariables : MonoBehaviour
{
    // Start is called before the first frame update

    public enum ECrosshairMode
    {
        CROSSHAIR_MODE_CROSS=0,
        CROSSHAIR_MODE_SQUARE,
        CROSSHAIR_MODE_MAX,
    }

    ECrosshairMode CrosshairMode = ECrosshairMode.CROSSHAIR_MODE_CROSS;

    [HideInInspector]
    public float flMouseSensitivity = 1.0f;

    [HideInInspector]
    public bool bDynamicFov = true;

    [HideInInspector]
    public bool bIsInteracting = false;

    [HideInInspector]
    public float flMasterVolume = 1.0f;

    [HideInInspector]
    public float flSfxVolume = 1.0f;

    [HideInInspector]
    public float flMusicVolume = 1.0f;

    [HideInInspector]
    public float flNarratorVolume = 1.0f;

    public GameObject SensitivitySliderObject;
    public GameObject DynamicFovToggleObject;
    public GameObject MasterVolumeObject;
    public GameObject SfxVolumeObject;
    public GameObject MusicVolumeObject;
    public GameObject NarratorVolumeObject;

    public GameObject CrosshairModeObject;

    public GameObject CrosshairGameObject;
    public UnityEngine.UI.Image CrosshairImage;
    public UnityEngine.Sprite CrosshairSquareNormal;
    public UnityEngine.Sprite CrosshairSquareInterac;
    public UnityEngine.Sprite CrosshairCrossNormal;
    public UnityEngine.Sprite CrosshairCrossInterac;
    //  public UI_Slider SensitivitySlider;

    // public UnityEngine.UI.Toggle DynamicFovToggle;


    void OnSliderUpdate()
    {
        flMouseSensitivity = SensitivitySliderObject.GetComponent<UI_Slider>().CurrentValue;
        flMasterVolume = MasterVolumeObject.GetComponent<UI_Slider>().CurrentValue / 100.0f;
        flSfxVolume = (SfxVolumeObject.GetComponent<UI_Slider>().CurrentValue / 100.0f) * flMasterVolume;
        flMusicVolume = (MusicVolumeObject.GetComponent<UI_Slider>().CurrentValue / 100.0f) * flMasterVolume;
        flNarratorVolume = (NarratorVolumeObject.GetComponent<UI_Slider>().CurrentValue / 100.0f) * flMasterVolume;
    }
    public void OnCrosshairModeChange(Int32 _CrosshairMode)
    {
        CrosshairMode = (ECrosshairMode)_CrosshairMode;
      
    }
    void Start()
    {
        
    }
    void Update()
    {
        // CrosshairMode = CrosshairModeObject.GetComponent<Dropdown>().value;

        //   Debug.Log(CrosshairMode);

        switch (CrosshairMode)
        {
            case ECrosshairMode.CROSSHAIR_MODE_CROSS:
                {
                    if (bIsInteracting)
                    {
                        CrosshairImage.sprite = CrosshairCrossInterac;
                    }
                    else
                    {
                        CrosshairImage.sprite = CrosshairCrossNormal;
                    }
                    break;
                }
            case ECrosshairMode.CROSSHAIR_MODE_SQUARE:
                {
                    if (bIsInteracting)
                    {
                        CrosshairImage.sprite = CrosshairSquareInterac;
                    }
                    else
                    {
                        CrosshairImage.sprite = CrosshairSquareNormal;
                    }
                    break;
                }
        }

        //can be optimized
        //Register Delegates to receive updates
        bDynamicFov = DynamicFovToggleObject.GetComponent<UnityEngine.UI.Toggle>().isOn;
        OnSliderUpdate();
    }
}

