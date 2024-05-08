using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDespawn : MonoBehaviour
{
    GameObject boxSpawn;
    private void Start()
    {
        boxSpawn = GameObject.FindGameObjectWithTag("BoxSpawn");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            Destroy(other.gameObject);
        }
    }
}
