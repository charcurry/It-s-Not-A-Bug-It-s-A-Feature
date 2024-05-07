using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class gatefake : MonoBehaviour
{

    // When the key collides with the door
    void OnCollisionEnter(Collision collision)
    {
        // Plays the particle system that each object has as a child, then deactivates both objects
        if (collision.gameObject.CompareTag("Key"))
        {
            collision.transform.GetComponent<Interactable>().DeactivateObject();
            collision.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

            transform.GetComponent<Collider>().enabled = false;
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
    }


}
