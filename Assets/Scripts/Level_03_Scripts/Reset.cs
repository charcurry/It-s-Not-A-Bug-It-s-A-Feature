using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    private Vector3 playerInitialPosition;
    private GetPosition[] objectsToReset;
    private GameObject heldObject;

    //Finds the players initial position, and the initial position of any object with the "GetPosition" script
    void Start()
    {
        playerInitialPosition = GameObject.FindWithTag("Player").transform.position;
        objectsToReset = FindObjectsOfType<GetPosition>();
    }

    //Puts everything where they started, if they are in the objectsToReset array
    public void ResetLevel()
    {
        List<GetPosition> newObjectsToReset = new List<GetPosition>();

        foreach (GetPosition obj in objectsToReset)
        {
            Interactable interactable = obj.GetComponent<Interactable>();
            if (interactable != null && !interactable.isPickedUp)
            {
                obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                obj.transform.position = obj.GetComponent<GetPosition>().initialPosition;
                obj.transform.rotation = obj.GetComponent<GetPosition>().initialRotation;
                newObjectsToReset.Add(obj);
            }
            else if (interactable != null && interactable.isPickedUp)
            {
                heldObject = obj.gameObject;
                GameObject newObject = Instantiate(heldObject, GameObject.FindWithTag("Player").transform.position, heldObject.GetComponent<GetPosition>().initialRotation);
                newObject.GetComponent<Rigidbody>().useGravity = true;
                //newObjectsToReset.Add(newObject.GetComponent<GetPosition>());
            }
        }
        GameObject.FindWithTag("Player").transform.position = playerInitialPosition;
        objectsToReset = newObjectsToReset.ToArray();
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
