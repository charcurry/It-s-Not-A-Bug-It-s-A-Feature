using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Reset : MonoBehaviour
{
    private Vector3 playerInitialPosition;
    private List<GetPosition> objectsToResetList = new List<GetPosition>(); // Use List instead of array
    private GameObject heldObject;
    public bool HasDuplicated;
    private CheckpointTrigger checkpoint;

    void Start()
    {
        playerInitialPosition = GameObject.FindWithTag("Player").transform.position;
        GetPosition[] objectsToResetArray = FindObjectsOfType<GetPosition>();
        objectsToResetList.AddRange(objectsToResetArray);
        checkpoint = FindObjectOfType<CheckpointTrigger>();
    }

    public void ResetLevel()
    {
        Vector3 playerPrevPosition = GameObject.FindWithTag("Player").transform.position;

        foreach (GetPosition obj in objectsToResetList)
        {
            Interactable interactable = obj.GetComponent<Interactable>();
            if (interactable != null && !interactable.isPickedUp)
            {
                obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                obj.transform.position = obj.GetComponent<GetPosition>().initialPosition;
                obj.transform.rotation = obj.GetComponent<GetPosition>().initialRotation;
            }
            else if (interactable != null && interactable.isPickedUp)
            {
                HasDuplicated = true;
                heldObject = obj.gameObject;
                GameObject newObject = Instantiate(heldObject, playerPrevPosition, heldObject.GetComponent<GetPosition>().initialRotation);
                newObject.GetComponent<Rigidbody>().useGravity = true;
                objectsToResetList.Add(newObject.GetComponent<GetPosition>());
                break; // Exit the loop after duplicating one object
            }
        }

        GameObject.FindWithTag("Player").transform.position = checkpoint.currentRespawnPoint.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetLevel();
            Debug.Log("Level Reset");
        }
    }
}