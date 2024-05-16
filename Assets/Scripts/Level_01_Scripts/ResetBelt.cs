using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBelt : Interactable
{
    private ConveyorBelt conveyorBelt;
    private GameObject conveyor;

    void Start()
    {
        // Find the GameObject with the tag Conveyor and get its ConveyorBelt script
        conveyor = GameObject.FindGameObjectWithTag("Conveyor");
        conveyorBelt = conveyor.GetComponent<ConveyorBelt>();
    }

    public override void interaction()
    {
        if (!canInteract())
            return;

        // Loop through all objects currently on the conveyor belt
        foreach (Rigidbody objRigidbody in conveyorBelt.objectsOnBelt.ToArray())
        {
            // Get the GameObject associated with the Rigidbody
            GameObject obj = objRigidbody.gameObject;

            // If the object is a box, destroy it
            if (obj.CompareTag("Box"))
            {
                Destroy(obj);
            }
        }
    }
}
