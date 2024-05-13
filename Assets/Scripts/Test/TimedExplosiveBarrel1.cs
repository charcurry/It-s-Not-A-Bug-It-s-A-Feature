using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedExplosiveBarreler : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float maxLaunchForce = 150f;
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // R key for testing
        {
            StartExplosionTimer();
        }

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
