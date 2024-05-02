using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeKill : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //moves player to entrance of second room
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = new Vector3(-20, 0, -45);
        }
    }
}
