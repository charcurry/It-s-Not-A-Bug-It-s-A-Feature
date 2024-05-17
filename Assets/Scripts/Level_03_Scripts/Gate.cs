using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    public string keyName;

    // When the key collides with the door
    void OnCollisionEnter(Collision collision)
    {
        // Plays the particle system that each object has as a child, then deactivates both objects
        if (collision.gameObject.CompareTag(keyName))
        {
            if (keyName == "KeyCard")
                SoundManager.PlaySound(SoundManager.Sound.Key_Card_Swipe, transform.position);
            else
            {
                SoundManager.PlaySound(SoundManager.Sound.Unlocking_Lock, transform.position);
            }
            collision.transform.GetComponent<Interactable>().DeactivateObject();
            collision.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

            transform.GetComponent<Collider>().enabled = false;
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
    }
}
