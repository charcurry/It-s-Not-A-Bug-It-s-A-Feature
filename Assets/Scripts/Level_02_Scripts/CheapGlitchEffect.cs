using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheapGlitchEffect : MonoBehaviour
{
    // Script creates a basic glitch effect by having two walls z fighting which randomly disables and enables
    [Header("Properties")]
    [SerializeField] private float speed = 20.0f;
    [SerializeField] private float maxOffset = 5.0f;
    [SerializeField] private float minOnTime = 0.4f;
    [SerializeField] private float maxOnTime = 0.7f;
    [SerializeField] private float minOffTime = 6.0f;
    [SerializeField] private float maxOffTime = 18.0f;

    private MeshRenderer meshRenderer;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Coroutine glitchCoroutine;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalPosition = transform.position;
        glitchCoroutine = StartCoroutine(GlitchEffect());
    }

    void Update()
    {
        if (meshRenderer.enabled)
        {
            MoveObject();
        }
    }

    private void MoveObject()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // If the wall has reached the position, choose a new random position
        if (transform.position == targetPosition)
        {
            float randomOffset = Random.Range(-maxOffset, maxOffset);
            targetPosition = new Vector3(originalPosition.x + randomOffset, originalPosition.y, originalPosition.z);
        }
    }

    private IEnumerator GlitchEffect()
    {
        while (true)
        {
            meshRenderer.enabled = true;

            // Set a new random target position and using random time movements
            float randomOffset = Random.Range(-maxOffset, maxOffset);
            targetPosition = new Vector3(originalPosition.x + randomOffset, originalPosition.y, originalPosition.z);
            yield return new WaitForSeconds(Random.Range(minOnTime, maxOnTime));
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(Random.Range(minOffTime, maxOffTime));
        }
    }

    // For when the electrical panel is damaged
    public void StopGlitchEffect()
    {
        if (glitchCoroutine != null)
        {
            StopCoroutine(glitchCoroutine);
            meshRenderer.enabled = false;
        }
    }
}
