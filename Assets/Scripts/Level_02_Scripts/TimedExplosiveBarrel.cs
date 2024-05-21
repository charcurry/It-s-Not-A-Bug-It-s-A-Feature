using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedExplosiveBarrel : Explodable
{
    [Header("Properties")]
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private float playerExplosionRadius = 5f;
    [SerializeField] private float maxLaunchForce = 125f;
    [SerializeField] private bool beginTimerOnStart = false;
    [SerializeField] private float timeToExplode = 15;

    [Header("References")]
    [SerializeField] GameObject explosionEffect;
    [SerializeField] MeshRenderer ledMeshRender;
    [SerializeField] GameObject ledLight;
    [SerializeField] Material ledOffMat;
    [SerializeField] Material ledOnMat;

    private float explosionTimeStamp;
    private float effectTimeStamp;
    private float effectCooldown;
    private bool isExplosionTimerStarted;
    private bool isLedOn;
    private bool isExploding = false;

    private void Start()
    {
        explosionTimeStamp = 0;
        isLedOn = false;

        if (beginTimerOnStart)
        {
            isExplosionTimerStarted = true;
            explosionTimeStamp = Time.time;
            effectTimeStamp = Time.time;
        }

        // Trigger to detect player proximity
        SphereCollider triggerCollider = gameObject.AddComponent<SphereCollider>();
        triggerCollider.isTrigger = true;
        triggerCollider.radius = playerExplosionRadius;
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.R)) // R key for testing
        {
            StartExplosionTimer();
        }
        */

        // If the timer on the barrel is going, once it reaches zero, explode
        if (isExplosionTimerStarted)
        {
            float timeRemaining = explosionTimeStamp + timeToExplode - Time.time;

            // After certain amounts of time have passed, the barrel's led flashs on and off quicker and it beeps faster
            switch (timeRemaining)
            {
                case >= 10:
                    effectCooldown = 1;
                    break;

                case < 10 when timeRemaining >= 5:
                    effectCooldown = 0.5f;
                    break;

                case < 5 when timeRemaining >= 1.5f:
                    effectCooldown = 0.2f;
                    break;

                case < 1.5f when timeRemaining > 0:
                    effectCooldown = 0.1f;
                    break;

                case <= 0:
                    Explode();
                    break;
            }

            if (effectTimeStamp + effectCooldown <= Time.time)
            {
                SoundManager.PlaySound(SoundManager.Sound.Timer_Beep, transform.position);
                effectTimeStamp = Time.time;

                if (isLedOn)
                {
                    ledLight.SetActive(false);
                    ledMeshRender.material = ledOffMat;
                    isLedOn = false;
                }
                else
                {
                    ledLight.SetActive(true);
                    ledMeshRender.material = ledOnMat;
                    isLedOn = true;
                }
            }
        }
    }

    // Starts the barrel's explosion timer
    public void StartExplosionTimer()
    {
        if (isExplosionTimerStarted)
            return;

        isExplosionTimerStarted = true;
        explosionTimeStamp = Time.time;
        effectTimeStamp = Time.time;
    }

    // Detect player entering trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartExplosionTimer();
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
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, playerExplosionRadius);

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Explodable>() && collider.gameObject != gameObject)
                collider.GetComponent<Explodable>().Explode();

            if (collider.GetComponent<Box>())
                collider.GetComponent<Box>().DestroyBox();

            if (collider.GetComponent<Rigidbody>())
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Calculate force based on distance to barrel
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    float forceMagnitude = Mathf.Lerp(maxLaunchForce, 0, distance / explosionRadius);

                    // Barrel explosion should only apply horizontal force, this is to prevent the player being launched straight through ceilings, into space
                    Vector3 forceDirection = (collider.transform.position - transform.position).normalized;
                    forceDirection.y = 0;
                    forceDirection.Normalize();

                    rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
                }
            }
        }

        foreach (var collider in playerColliders)
        {
            if (collider.CompareTag("Player"))
            {
                Debug.Log(Vector3.Distance(collider.transform.position, transform.position));
                if (Vector3.Distance(collider.transform.position, transform.position) < 2)
                {
                    collider.GetComponent<PlayerController>().SetClip(true);
                }

                if (collider.GetComponent<Rigidbody>())
                {
                    Rigidbody rb = collider.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        // Calculate force based on distance to barrel
                        float distance = Vector3.Distance(transform.position, collider.transform.position);
                        float forceMagnitude = Mathf.Lerp(maxLaunchForce, 0, distance / playerExplosionRadius);

                        // Barrel explosion should only apply horizontal force, this is to prevent the player being launched straight through ceilings, into space
                        Vector3 forceDirection = (collider.transform.position - transform.position).normalized;
                        forceDirection.y = 0;
                        forceDirection.Normalize();

                        rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
                    }
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NailProjectile"))
        {
            Explode();
        }
    }
}
