using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    private Vector3 playerInitialPosition;
    private GetPosition[] objectsToReset;

    void Start()
    {
        playerInitialPosition = GameObject.FindWithTag("Player").transform.position;
        objectsToReset = FindObjectsOfType<GetPosition>();
    }

    public void ResetLevel()
    {
        GameObject.FindWithTag("Player").transform.position = playerInitialPosition;

        foreach (GetPosition obj in objectsToReset)
        {
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero; 
            obj.transform.position = obj.GetComponent<GetPosition>().initialPosition; 
            obj.transform.rotation = obj.GetComponent<GetPosition>().initialRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetLevel();
        }
    }


}
