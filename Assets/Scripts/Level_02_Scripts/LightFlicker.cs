using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private List<Light> spotLights;
    [SerializeField] private float flickerDuration = 3.0f;
    [SerializeField] private float flickerInterval = 0.1f;

    public void StartFlickering()
    {
        StartCoroutine(FlickerLights());
    }

    private IEnumerator FlickerLights()
    {
        float endTime = Time.time + flickerDuration;

        while (Time.time < endTime)
        {
            foreach (Light light in spotLights)
            {
                light.enabled = Random.value > 0.5f;
            }

            yield return new WaitForSeconds(flickerInterval);
        }

        // Make sure they are on when the flicker ends
        foreach (Light light in spotLights)
        {
            light.enabled = true;
        }
    }
}
