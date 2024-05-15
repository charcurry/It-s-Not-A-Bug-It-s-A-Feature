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
        if (door.isOpen)
        {
            if (resume)
            {
                beltController.conveyorBelts.Add(belt);
                resume = false;
            }
        }
        else
        {
            if (!resume)
            {
                beltController.conveyorBelts.Remove(belt);
                resume = true;
            }
        }
    }

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
        if (collision.gameObject.CompareTag("Box") && CheckIfObjectOnBelt())
        {
            beltController.conveyorBelts.Remove(belt);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box") && CheckIfObjectOnBelt() && door.isOpen == true)
        {
            beltController.conveyorBelts.Add(belt);
        }
    }
}