using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBox : MonoBehaviour
{
    private Interactable interactable;
    private BoxInstantiation boxInstantiation;
    private GameObject boxSpawn;
    [HideInInspector] public bool isSpiked;
    private void Start()
    {
        isSpiked = false;

        // Find the GameObject with the tag BoxSpawn and get its BoxInstantiation script
        boxSpawn = GameObject.FindGameObjectWithTag("BoxSpawn");
        boxInstantiation = boxSpawn.GetComponent<BoxInstantiation>();
    }

    private void OnCollisionStay(Collision collision)
    {
        interactable = collision.gameObject.GetComponent<Interactable>();

        // If the colliding GameObject is a box and it's picked up
        if (collision.gameObject.CompareTag("Box") && interactable.isPickedUp == true)
        {
            // Unfreeze constraints on the box Rigidbody
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            // Spike is deactivated
            isSpiked = false;
        }
        // If the colliding GameObject is a box and it's not picked up
        else if (collision.gameObject.CompareTag("Box") && interactable.isPickedUp == false)
        {
            // Freeze all constraints on the box Rigidbody and freeze rotation
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            collision.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
            // Set the rotation of the Rigidbody to (0,currentYRotation,0)
            float currentYRotation = collision.gameObject.GetComponent<Rigidbody>().rotation.eulerAngles.y;
            Quaternion newRotation = Quaternion.Euler(0, currentYRotation, 0);
            collision.gameObject.GetComponent<Rigidbody>().rotation = newRotation;
            // Spike is activated
            isSpiked = true;
        }
    }
}
