using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject explosionEffect;

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

        Destroy(gameObject);
    }
}
