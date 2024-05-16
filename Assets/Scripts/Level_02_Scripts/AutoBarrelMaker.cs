using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBarrelMaker : MonoBehaviour
{
    // Script that auto creates one red barrel, if barrel has exploded, a new one will spawn
    [Header("Properties")]
    [SerializeField] private float launchForce = 5f;
    [SerializeField] private float spawnDelay = 1.5f;

    [Header("References")]
    [SerializeField] private GameObject barrelPrefab;
    [SerializeField] private Transform spawnPoint;

    private GameObject currentBarrel;
    private bool isSpawning;

    void Start()
    {
        CreateAndLaunchBarrel();
    }

    // Check if a new barrel needs to spawn
    void Update()
    {
        if (currentBarrel == null && !isSpawning)
        {
            StartCoroutine(DelaySpawn(spawnDelay));
        }
    }

    // Spawn and launch the barrel from the spawn point
    private void CreateAndLaunchBarrel()
    {
        currentBarrel = Instantiate(barrelPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = currentBarrel.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
        }
        isSpawning = false;
    }

    // Delay spawning
    private IEnumerator DelaySpawn(float delay)
    {
        isSpawning = true;
        yield return new WaitForSeconds(delay);
        CreateAndLaunchBarrel();
    }
}
