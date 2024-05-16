using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBelt : MonoBehaviour
{
    ConveyorBeltController beltController;
    ConveyorBelt belt;
    Door door;
    public GameObject conveyor;
    public GameObject doorButton;
    private bool resume = false;

    void Start()
    {
        door = doorButton.GetComponent<Door>();
        beltController = FindObjectOfType<ConveyorBeltController>();
        belt = conveyor.GetComponent<ConveyorBelt>();
    }

    void Update()
    {
        // If the door is open resume the belt if it was last paused
        if (door.isOpen)
        {
            if (resume)
            {
                beltController.conveyorBelts.Add(belt);
                resume = false;
            }
        }
        // If the door is closed pause the belt if it was last running
        else
        {
            if (!resume)
            {
                beltController.conveyorBelts.Remove(belt);
                resume = true;
            }
        }
    }

    // Check if there are objects (boxes) on the belt
    private bool CheckIfObjectOnBelt()
    {
        foreach (Rigidbody objRb in belt.objectsOnBelt)
        {
            if (objRb != null && objRb.gameObject.CompareTag("Box"))
            {
                return true;
            }
        }
        return false;
    }

    private void OnCollisionStay(Collision collision)
    {
        // If a box is on the belt and the door is closed, pause the belt
        if (collision.gameObject.CompareTag("Box") && CheckIfObjectOnBelt())
        {
            beltController.conveyorBelts.Remove(belt);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // If a box exits the belt and the door is open, resume the belt
        if (collision.gameObject.CompareTag("Box") && CheckIfObjectOnBelt() && door.isOpen == true)
        {
            beltController.conveyorBelts.Add(belt);
        }
    }
}