using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
    // Private variables
    private Slider slider;
    private TMP_Text valueText;

    [Header("Properties")]
    [SerializeField] private float minValue = 0;
    [SerializeField] private float maxValue = 100;
    [SerializeField] public float currentValue = 0;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();

        Transform valueTextTransform = transform.Find("Value");

        if (valueTextTransform != null)
        {
            valueText = valueTextTransform.GetComponent<TMP_Text>();
        }

        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = currentValue;
        slider.onValueChanged.AddListener(delegate { OnValueChange(); });
        UpdateText();
    }

    void UpdateText()
    {
        if (valueText != null)
        {
            float roundedValue = Mathf.Round(slider.value * 100) / 100;
            valueText.text = roundedValue.ToString("F2");
        }
    }

    void OnValueChange()
    {
        currentValue = slider.value;
        UpdateText();
    }
}