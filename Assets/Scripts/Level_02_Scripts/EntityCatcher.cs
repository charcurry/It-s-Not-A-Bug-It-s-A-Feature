using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCatcher : MonoBehaviour
{
    // This script just tp's anything that has somehow falling out of the map back into the first room
    public Transform eTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (eTarget != null)
        {
            other.transform.position = eTarget.position;

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
