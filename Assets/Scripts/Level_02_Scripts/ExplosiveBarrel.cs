using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject explosionEffect;
    public float explosionRadius = 5f;
    public float maxLaunchForce = 150f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // R key for testing
        {
            Explode();
        }
    }

    void Explode()
    {
        SoundManager.PlaySound(SoundManager.Sound.Explosion, transform.position);

        if (explosionEffect != null)
        {
            GameObject explosionInstance = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosionInstance, 1f);
        }

        // Find player or anything with a rigidbody or collider
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

            // Tried using mesh render but just replacing the panel with the damage one works fine

            if (collider.gameObject.name == "Electrical Panel")
            {
                Vector3 panelPosition = collider.transform.position;
                collider.gameObject.SetActive(false);

                GameObject damagedPanel = GameObject.Find("Electrical Panel Damaged");
                if (damagedPanel != null)
                {
                    damagedPanel.transform.position = panelPosition;
                    damagedPanel.SetActive(true);
                }

                // Second door to open
                GameObject door = GameObject.FindGameObjectWithTag("SecondDoor");
                if (door != null)
                {
                    door.GetComponent<OpenDoor>().Open();
                }

                // And finally, the light changes color to green
                GameObject damageLight = GameObject.FindGameObjectWithTag("SecondDoorLight");
                if (damageLight != null)
                {
                    Light lightComponent = damageLight.GetComponent<Light>();
                    if (lightComponent != null)
                    {
                        lightComponent.color = Color.green;
                    }
                }
            }

            // For the glass box blocking the last button
            if (collider.gameObject.tag == "GlassBox")
            {
                collider.gameObject.SetActive(false);
            }
        }

        Destroy(gameObject);
    }
}
