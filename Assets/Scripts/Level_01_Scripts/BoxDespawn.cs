using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDespawn : MonoBehaviour
{
    BoxInstantiation boxInstantiation;
    GameObject boxSpawn;
    private void Start()
    {
        boxSpawn = GameObject.FindGameObjectWithTag("BoxSpawn");
        boxInstantiation = boxSpawn.GetComponent<BoxInstantiation>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            Destroy(other.gameObject);
            boxInstantiation.boxCount -= 1;
        }
    }
}
