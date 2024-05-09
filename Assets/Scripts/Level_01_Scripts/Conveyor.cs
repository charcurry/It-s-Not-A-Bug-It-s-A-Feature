using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltController : MonoBehaviour
{
    public float conveyorSpeed;
    public float objectSpeed;
    public List<ConveyorBelt> conveyorBelts = new List<ConveyorBelt>();

    private void FixedUpdate()
    {
        foreach (ConveyorBelt belt in conveyorBelts)
        {
            belt.MoveBelt(objectSpeed * Time.fixedDeltaTime);
            belt.MoveMaterial(conveyorSpeed * Time.fixedDeltaTime);
        }
    }
}