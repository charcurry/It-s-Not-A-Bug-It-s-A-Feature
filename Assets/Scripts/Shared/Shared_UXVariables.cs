using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shared_UXVariables : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector]
    public float flMouseSensitivity = 1.0f;

    [HideInInspector]
    public bool bDynamicFov = true;

    [HideInInspector]
    public bool bIsInteracting = false;

    public GameObject SensitivitySliderObject;

    public GameObject DynamicFovToggleObject;


  //  public UI_Slider SensitivitySlider;

   // public UnityEngine.UI.Toggle DynamicFovToggle;


    void Start()
    {
        
    }
    void Update()
    {
        bDynamicFov = DynamicFovToggleObject.GetComponent<UnityEngine.UI.Toggle>().isOn;
        flMouseSensitivity = SensitivitySliderObject.GetComponent<UI_Slider>().CurrentValue;

    }
}
