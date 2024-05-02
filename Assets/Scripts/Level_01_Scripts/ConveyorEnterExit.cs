using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorEnterExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //moves player to the entrance of room
        if(other.CompareTag("Player"))
        {
            other.transform.position = new Vector3(-20, 0, -5);
        }
    }
}
