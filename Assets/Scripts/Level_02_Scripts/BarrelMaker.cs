using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelMaker : MonoBehaviour
{
    public GameObject Object;
    public float launchForce = 10f;

    void Update()
    {
        // G key for testing
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Create the barrel
            GameObject newObj = Instantiate(Object, transform.position, Quaternion.identity);

            // Get Rigidbody component and apply an upward force
            Rigidbody rb = newObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("no rigidbody bruh");
            }
        }
    }
}
