using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelMaker : Interactable
{
    public GameObject barrelObject;
    public Transform spawnPoint;
    public float launchForce = 10f;
    public float spawnDelay = 1.5f;

    private IEnumerator SpawnBarrel()
    {
        yield return new WaitForSeconds(spawnDelay);

        if (spawnPoint != null)
        {
            GameObject barrel = Instantiate(barrelObject, spawnPoint.position, spawnPoint.rotation);
            Rigidbody rb = barrel.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("no rigidbody bruh");
            }
        }
        else
        {
            Debug.LogError("no spawn point");
        }
    }

    public override void interaction()
    {
        if (!canInteract())
            return;

        StartCoroutine(SpawnBarrel());
    }
}
