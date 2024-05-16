using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [HideInInspector] public bool isObjectHere = false;
    [HideInInspector] public List<Collider> colliderList = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Collider>().isTrigger)
            colliderList.Add(other);

        if (colliderList.Count > 0)
            isObjectHere = true; 
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Collider>().isTrigger)
            colliderList.Remove(other);

        if (colliderList.Count < 1)
            isObjectHere = false;
    }
}
