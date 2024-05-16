using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailProjectile : MonoBehaviour
{
    // Makes the nails stick in things
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;

        transform.parent = collision.transform;

        GetComponent<Collider>().enabled = false;
    }
}
