using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedExplosiveBarrel : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float maxLaunchForce = 150f;
    [SerializeField] private bool beginTimerOnStart = true;
    [SerializeField] private float timeToExplode = 15;

    [Header("References")]
    [SerializeField] GameObject explosionEffect;
    [SerializeField] MeshRenderer ledMeshRender;
    [SerializeField] GameObject ledLight;
    [SerializeField] Material ledOffMat;
    [SerializeField] Material ledOnMat;

    private float timeStamp;
    private float effectTimeStamp;
    private float effectCooldown;
    private bool isTimerStarted;
    private bool isLedOn;

    private void Start()
    {
        timeStamp = 0;
        isLedOn = true;

        if (beginTimerOnStart)
        {
            isTimerStarted = true;
            timeStamp = Time.time;
            effectTimeStamp = Time.time;
        }
    }

    private void Update()
    {
        if (isTimerStarted)
        {
            float timeRemaining = timeStamp + timeToExplode - Time.time;

            switch (timeRemaining) 
            {
                case >= 10:
                    effectCooldown = 1;
                    break;

                case < 10 when timeRemaining >= 5:
                    effectCooldown = 0.7f;
                    break;

                case < 5 when timeRemaining >= 1:
                    effectCooldown = 0.45f;
                    break;

                case < 1 when timeRemaining > 0:
                    effectCooldown = 0.15f;
                    break;

                case <= 0:
                    Explode();
                    break;
            }

            if (effectTimeStamp + effectCooldown <= Time.time)
            {
                // ADD BEEPING SOUND HERE
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

    public void StartTimer()
    {
        isTimerStarted = true;
        timeStamp = Time.time;
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
