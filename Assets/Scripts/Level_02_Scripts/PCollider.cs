using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCollider : MonoBehaviour
{
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
