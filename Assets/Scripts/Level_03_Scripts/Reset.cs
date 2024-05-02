using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    private Vector3 playerInitialPosition;
    private GetPosition[] objectsToReset;


    //Finds the players initial position, and the initial position of any object with the "GetPosition" script
    void Start()
    {
        playerInitialPosition = GameObject.FindWithTag("Player").transform.position;
        objectsToReset = FindObjectsOfType<GetPosition>();
    }

    //Puts eveerything where they started, if they are in the objectsToReset array
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

    //!PlaceHolder! resets the level if the player triggers the resetter
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetLevel();
        }
    }


}
