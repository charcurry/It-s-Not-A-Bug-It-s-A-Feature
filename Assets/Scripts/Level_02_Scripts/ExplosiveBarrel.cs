using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject explosionEffect;
    public float explosionRadius = 5f;
    public float maxLaunchForce = 100f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // R key for testing
        {
            Explode();
        }
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosionInstance = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosionInstance, 1f);
        }

        // Find player or anything with a rigidbody
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate force based on distance to barrel
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                float forceMagnitude = Mathf.Lerp(maxLaunchForce, 0, distance / explosionRadius);
                Vector3 forceDirection = (collider.transform.position - transform.position).normalized;
                rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
            }
        }

        Destroy(gameObject);
    }
}
