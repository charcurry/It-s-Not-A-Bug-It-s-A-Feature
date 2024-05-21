using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    // Script that auto creates one cardboard box, if it is destroyed, a new one will spawn
    [Header("Properties")]
    [SerializeField] private float launchForce = 5f;
    [SerializeField] private float spawnDelay = 1.5f;

    [Header("References")]
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private Transform spawnPoint;

    private GameObject currentBox;
    private bool isSpawning;

    void Start()
    {
        CheckAndCreateBox();
    }

    // Check if a new box needs to spawn
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("ButtonBox") == null && !isSpawning)
        {
            StartCoroutine(DelaySpawn(spawnDelay));
        }
    }

    // Check and create the box
    private void CheckAndCreateBox()
    {
        if (GameObject.FindGameObjectWithTag("ButtonBox") == null)
        {
            CreateAndLaunchBox();
        }
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
        isSpawning = false;
    }

    // Delay spawning
    private IEnumerator DelaySpawn(float delay)
    {
        isSpawning = true;
        yield return new WaitForSeconds(delay);
        CreateAndLaunchBox();
    }
}
