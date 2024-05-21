using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    private Renderer platformRenderer;
    private Collider platformCollider;
    private bool isDisappeared = false;

    void Start()
    {
        platformRenderer = GetComponent<Renderer>();
        platformCollider = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDisappeared)
        {
            StartCoroutine(DisappearAndReappear());
        }
    }

    private IEnumerator DisappearAndReappear()
    {
        isDisappeared = true;
        yield return new WaitForSeconds(1.0f);

        platformRenderer.enabled = false;
        platformCollider.enabled = false;

        yield return new WaitForSeconds(3.0f);

        platformRenderer.enabled = true;
        platformCollider.enabled = true;

        isDisappeared = false;
    }
}
