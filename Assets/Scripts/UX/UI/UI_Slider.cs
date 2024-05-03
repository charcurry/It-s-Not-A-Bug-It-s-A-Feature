using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Slider : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider Slider;
    private TMP_Text ValueText;

    public float MinValue = 0;
    public float MaxValue = 100;
    public float CurrentValue = 0;

    void Start()
    {
        Slider = gameObject.GetComponent<Slider>();

        Transform valueTextTransform = transform.Find("Value");

        if (valueTextTransform != null)
            ValueText = valueTextTransform.GetComponent<TMP_Text>();
        
        Slider.minValue = MinValue;
        Slider.maxValue = MaxValue;
        // Set the initial value
        Slider.value = CurrentValue;
        Slider.onValueChanged.AddListener(delegate { OnValueChange(); });
        // Update the text when starting
        UpdateText();
    }

    public void UpdateText()
    {
        // Assuming you have a TMP_Text component assigned to ValueText
        if (ValueText != null)
        {
            float roundedValue = Mathf.Round(Slider.value * 100) / 100; 
            ValueText.text = roundedValue.ToString("F2"); 
        }
    }
    public void OnValueChange()
    {
        CurrentValue = Slider.value;
        // Update text whenever the value changes
        UpdateText();
    }

}
