using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltController : MonoBehaviour
{
    [Header("Properties")]
    public float conveyorSpeed;
    public float objectSpeed;
    [HideInInspector] public List<ConveyorBelt> conveyorBelts = new List<ConveyorBelt>();

    private void FixedUpdate()
    {
        // Go through each conveyor belt in the list
        foreach (ConveyorBelt belt in conveyorBelts)
        {
            // Move the belt and its material texture based on specified speeds and fixed delta time
            belt.MoveBelt(objectSpeed * Time.fixedDeltaTime);
            belt.MoveMaterial(conveyorSpeed * Time.fixedDeltaTime);
        }
    }
}
