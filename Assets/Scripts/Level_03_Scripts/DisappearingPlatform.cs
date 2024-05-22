using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    private Renderer platformRenderer;
    private Renderer fanRenderer;
    private Collider platformCollider;
    private bool isDisappeared = false;

    void Start()
    {
        platformRenderer = GetComponent<Renderer>();
        platformCollider = GetComponent<Collider>();
        fanRenderer = transform.GetChild(0).GetComponent<Renderer>();
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
        fanRenderer.enabled = false;
        platformCollider.enabled = false;

        yield return new WaitForSeconds(3.0f);

        platformRenderer.enabled = true;
        fanRenderer.enabled = true;
        platformCollider.enabled = true;

        isDisappeared = false;
    }
}
