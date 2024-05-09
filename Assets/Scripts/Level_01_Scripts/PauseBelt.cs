using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBelt : MonoBehaviour
{
    ConveyorBeltController beltController;
    ConveyorBelt belt;
    public GameObject conveyor;

    void Start()
    {
        beltController = FindObjectOfType<ConveyorBeltController>();
        belt = conveyor.GetComponent<ConveyorBelt>();
    }

    void Update()
    {

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
        if (collision.gameObject.CompareTag("Box") && CheckIfObjectOnBelt())
        {
            beltController.conveyorBelts.Add(belt);
        }
    }
}