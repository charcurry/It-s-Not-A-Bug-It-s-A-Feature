using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private string keyName;

    [HideInInspector] public bool isUnlocked;

    private void Start()
    {
        isUnlocked = false;
    }

    // When the key collides with the door
    void OnCollisionEnter(Collision collision)
    {
        // Plays the particle system that each object has as a child, then deactivates both objects
        if (collision.gameObject.CompareTag(keyName))
        {
            SoundManager.PlaySound(SoundManager.Sound.Unlocking_Door, transform.position);
            collision.transform.GetComponent<Interactable>().DeactivateObject();
            collision.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

            isUnlocked = true;

            transform.GetComponent<Collider>().enabled = false;
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
    }
}
