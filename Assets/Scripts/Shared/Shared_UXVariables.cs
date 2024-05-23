using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Shared_UXVariables;
using static SoundManager;

public class Shared_UXVariables : MonoBehaviour
{
    // Start is called before the first frame update

    public enum CrosshairMode
    {
        CrosshairModeCross = 0,
        CrosshairModeSquare,
        CrosshairModeMax,
    }

    private CrosshairMode crosshairMode = CrosshairMode.CrosshairModeCross;

    [HideInInspector] public float mouseSensitivity = 1.0f;
    [HideInInspector] public bool dynamicFov = true;
    [HideInInspector] public bool isInteracting = false;
    [HideInInspector] public float masterVolume = 1.0f;
    [HideInInspector] public float sfxVolume = 1.0f;
    [HideInInspector] public float musicVolume = 1.0f;
    [HideInInspector] public float narratorVolume = 1.0f;

    [Header("References")]
    public GameObject sensitivitySliderObject;
    public GameObject dynamicFovToggleObject;
    public GameObject masterVolumeObject;
    public GameObject sfxVolumeObject;
    public GameObject musicVolumeObject;
    public GameObject narratorVolumeObject;
    public GameObject crosshairModeObject;

    public GameObject gameAssetsManager;

    public GameObject crosshairGameObject;
    public UnityEngine.UI.Image crosshairImage;
    public UnityEngine.UI.Image crosshairControlsNormalImage;
    public UnityEngine.UI.Image crosshairControlsInteracImage;
    public UnityEngine.Sprite crosshairSquareNormal;
    public UnityEngine.Sprite crosshairSquareInterac;
    public UnityEngine.Sprite crosshairCrossNormal;
    public UnityEngine.Sprite crosshairCrossInterac;

    public float GetSfxVolume()
    {
        return sfxVolume;
    }

    void Start()
    {
        // Initialization code here if needed
    }

    void Update()
    {
        switch (crosshairMode)
        {
            case CrosshairMode.CrosshairModeCross:
                crosshairControlsNormalImage.sprite = crosshairCrossNormal;
                crosshairControlsInteracImage.sprite = crosshairCrossInterac;

                if (isInteracting)
                {
                    crosshairImage.sprite = crosshairCrossInterac;
                }
                else
                {
                    crosshairImage.sprite = crosshairCrossNormal;
                }
                break;

            case CrosshairMode.CrosshairModeSquare:
                crosshairControlsNormalImage.sprite = crosshairSquareNormal;
                crosshairControlsInteracImage.sprite = crosshairSquareInterac;

                if (isInteracting)
                {
                    crosshairImage.sprite = crosshairSquareInterac;
                }
                else
                {
                    crosshairImage.sprite = crosshairSquareNormal;
                }
                break;
        }

        dynamicFov = dynamicFovToggleObject.GetComponent<UnityEngine.UI.Toggle>().isOn;
        OnSliderUpdate();
    }

    void OnSliderUpdate()
    {
        //gameAssetsManager.GetComponent<GameAssets>().soundSettingsDictionary.Clear();
        //gameAssetsManager.GetComponent<GameAssets>().InitializeSoundSettings();
        gameAssetsManager.GetComponent<GameAssets>().backGroundMusic.volume = musicVolume;
        AirVent[] airVents = FindObjectsOfType<AirVent>();
        foreach (AirVent airVent in airVents)
        {
            airVent.audioSource.volume = sfxVolume;
        }

        mouseSensitivity = sensitivitySliderObject.GetComponent<UISlider>().currentValue;
        masterVolume = masterVolumeObject.GetComponent<UISlider>().currentValue / 100.0f;
        sfxVolume = (sfxVolumeObject.GetComponent<UISlider>().currentValue / 100.0f) * masterVolume;
        musicVolume = (musicVolumeObject.GetComponent<UISlider>().currentValue / 100.0f) * masterVolume;
        narratorVolume = (narratorVolumeObject.GetComponent<UISlider>().currentValue / 100.0f) * masterVolume;
    }

    public void OnCrosshairModeChange(TMP_Dropdown dropDown)
    {
        crosshairMode = (CrosshairMode)dropDown.value;
    }
}

