using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyHarzard : MonoBehaviour
{
    // If the player touches an object with this script, they die
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerController>().Kill();
        }
    }

    // If the player touches a trigger with this script, they die
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerController>().Kill();
        }
    }
}
