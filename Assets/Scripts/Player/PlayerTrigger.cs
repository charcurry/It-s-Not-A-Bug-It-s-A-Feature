using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [HideInInspector] public bool isObjectHere = false; 
    int triggerObjects = 0;

    private void OnTriggerEnter(Collider other)
    {
        triggerObjects++;

        if (triggerObjects > 0)
            isObjectHere = true; 
    }

    private void OnTriggerExit(Collider other)
    {
        triggerObjects--;

        if (triggerObjects < 1)
            isObjectHere = false;
    }

}
