using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeKill : MonoBehaviour
{
    Interactable interactable;
    BoxInstantiation boxInstantiation;
    GameObject boxSpawn;
    public bool isSpiked;
    private void Start()
    {
        isSpiked = false;
        boxSpawn = GameObject.FindGameObjectWithTag("BoxSpawn");
        boxInstantiation = boxSpawn.GetComponent<BoxInstantiation>();
    }

    private void OnCollisionStay(Collision collision)
    {
        interactable = collision.gameObject.GetComponent<Interactable>();
        if (collision.gameObject.CompareTag("Box") && interactable.isPickedUp == true)
        {
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            isSpiked = false;
        }
        else if (collision.gameObject.CompareTag("Box") && interactable.isPickedUp == false)
        {
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            collision.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
            isSpiked = true;
            boxInstantiation.boxCount -= 1;
        }
    }
}
