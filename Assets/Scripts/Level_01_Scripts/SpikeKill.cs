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
        interactable = GameObject.FindGameObjectWithTag("Box").GetComponent<Interactable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //moves player to entrance of second room
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = new Vector3(-20, 0, -45);
        }
        if(collision.gameObject.CompareTag("Box") && interactable.isPickedUp == true)
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
