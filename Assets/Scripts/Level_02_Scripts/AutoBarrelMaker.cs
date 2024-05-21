using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBarrelMaker : MonoBehaviour
{
    // Script that auto creates one red barrel, if barrel has exploded, a new one will spawn
    [Header("Properties")]
    [SerializeField] private float spawnDelay = 1.5f;
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private Vector3 platformOffset;

    [Header("References")]
    [SerializeField] private GameObject barrelPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform platform;

    private Vector3 initialPlatformPosition;
    private GameObject currentBarrel;
    private bool isSpawning;

    void Start()
    {
        initialPlatformPosition = platform.position;
        StartCoroutine(SpawnBarrelProcess());
    }

    void Update()
    {
        if (currentBarrel == null && !isSpawning)
        {
            StartCoroutine(SpawnBarrelProcess());
        }
    }

    private IEnumerator SpawnBarrelProcess()
    {
        isSpawning = true;

        // Move platform down
        yield return MovePlatform(initialPlatformPosition + platformOffset, moveDuration);

        // Wait for the spawn delay
        yield return new WaitForSeconds(spawnDelay);

        // Spawn barrel
        currentBarrel = Instantiate(barrelPrefab, spawnPoint.position, spawnPoint.rotation);

        // Move platform up
        yield return MovePlatform(initialPlatformPosition, moveDuration);

        isSpawning = false;
    }

    private IEnumerator MovePlatform(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = platform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            platform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        platform.position = targetPosition;
    }
}
