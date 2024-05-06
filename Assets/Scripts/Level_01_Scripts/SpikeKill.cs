using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeKill : MonoBehaviour
{
    BoxInstantiation boxInstantiation;
    GameObject boxSpawn;
    private void Start()
    {
        boxSpawn = GameObject.FindGameObjectWithTag("BoxSpawn");
        boxInstantiation = boxSpawn.GetComponent<BoxInstantiation>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //moves player to entrance of second room
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = new Vector3(-20, 0, -45);
        }
        if (collision.gameObject.CompareTag("Box"))
        {
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            boxInstantiation.boxCount -= 1;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
