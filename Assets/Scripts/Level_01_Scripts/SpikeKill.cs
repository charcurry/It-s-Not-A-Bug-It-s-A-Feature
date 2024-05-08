using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeKill : MonoBehaviour
{
    Interactable interactable;
    BoxInstantiation boxInstantiation;
    GameObject boxSpawn;
    private void Start()
    {
        boxSpawn = GameObject.FindGameObjectWithTag("BoxSpawn");
        boxInstantiation = boxSpawn.GetComponent<BoxInstantiation>();
    }

    private void OnCollisionStay(Collision collision)
    {
        interactable = collision.gameObject.GetComponent<Interactable>();
        if (collision.gameObject.CompareTag("Box") && interactable.isPickedUp == true)
        {
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else if (collision.gameObject.CompareTag("Box") && interactable.isPickedUp == false)
        {
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            boxInstantiation.boxCount -= 1;
        }
    }
}
