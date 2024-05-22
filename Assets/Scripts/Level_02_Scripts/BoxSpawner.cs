using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    // Script that auto creates one cardboard box, if it is destroyed, a new one will spawn
    [Header("Properties")]
    [SerializeField] private float launchForce = 5f;
    [SerializeField] private float spawnDelay = 1.5f;
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private Vector3 platformOffset;

    [Header("References")]
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform platform;

    private Vector3 initialPlatformPosition;
    private GameObject currentBox;
    private bool isSpawning;

    void Start()
    {
        initialPlatformPosition = platform.position;
        CheckAndCreateBox();
    }

    // Check if a new box needs to spawn
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("ButtonBox") == null && !isSpawning)
        {
            StartCoroutine(SpawnBoxProcess());
        }
    }

    // Spawning the box
    private IEnumerator SpawnBoxProcess()
    {
        isSpawning = true;

        // Move platform down
        yield return MovePlatform(initialPlatformPosition + platformOffset, moveDuration);

        // Wait for the spawn delay
        yield return new WaitForSeconds(spawnDelay);

        // Spawn box
        CreateAndLaunchBox();

        // Move platform up
        yield return MovePlatform(initialPlatformPosition, moveDuration);

        isSpawning = false;
    }

    // Spawn and launch the box from the spawn point
    private void CreateAndLaunchBox()
    {
        currentBox = Instantiate(boxPrefab, spawnPoint.position, spawnPoint.rotation);
        currentBox.tag = "ButtonBox";
        Rigidbody rb = currentBox.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
        }
    }

    // Move platform
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

    // Check for box if not in scene or something idk
    private void CheckAndCreateBox()
    {
        if (GameObject.FindGameObjectWithTag("ButtonBox") == null)
        {
            StartCoroutine(SpawnBoxProcess());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
        }
        else
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
