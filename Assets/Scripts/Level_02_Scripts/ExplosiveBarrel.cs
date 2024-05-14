using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : Explodable
{
    [Header("Properties")]
    public float explosionRadius = 5f;
    public float maxLaunchForce = 150f;

    [Header("References")]
    public GameObject explosionEffect;

    private bool isExploding = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // R key for testing
        {
            Explode();
        }
    }

    public override void Explode()
    {
        if (isExploding)
            return;

        isExploding = true;

        SoundManager.PlaySound(SoundManager.Sound.Explosion, transform.position);

        GameObject explosionInstance = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosionInstance, 1f);

        // Find player or anything with a rigidbody or collider
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Explodable>() && collider.gameObject != gameObject)
                collider.GetComponent<Explodable>().Explode();

            if (collider.GetComponent<Rigidbody>())
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
        }

        Destroy(gameObject);
    }
}
