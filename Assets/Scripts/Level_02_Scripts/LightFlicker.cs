using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float flickerDuration = 3.0f;
    [SerializeField] private float flickerInterval = 0.1f;

    [Header("References")]
    [SerializeField] private List<Light> spotLights;
    [SerializeField] private List<Renderer> gameObjects;
    [SerializeField] private Material onMaterial;
    [SerializeField] private Material offMaterial;

    // Start flickering
    public void StartFlickering()
    {
        StartCoroutine(FlickerLights());
    }

    // Flicker the lights and change the materials of overhead light objects
    private IEnumerator FlickerLights()
    {
        float endTime = Time.time + flickerDuration;

        while (Time.time < endTime)
        {
            foreach (Light light in spotLights)
            {
                bool isOn = Random.value > 0.5f;
                light.enabled = isOn;

                foreach (Renderer obj in gameObjects)
                {
                    obj.material = isOn ? onMaterial : offMaterial;
                }
            }

            yield return new WaitForSeconds(flickerInterval);
        }

        // Make sure lights are off when the flicker ends
        foreach (Light light in spotLights)
        {
            light.enabled = false;
        }

        // Set materials to off when flicker ends cause no power
        foreach (Renderer obj in gameObjects)
        {
            obj.material = offMaterial;
        }
    }
}
