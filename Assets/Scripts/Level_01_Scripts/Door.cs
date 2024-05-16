using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    // References to door and conveyor door
    [Header("References")]
    [SerializeField] private Transform door;
    [SerializeField] private Transform conveyorDoor;

    // Positions for the door and conveyor door closed and open
    [Header("Properties")]
    [SerializeField] private Vector3 closedPos;
    [SerializeField] private Vector3 openPos;
    [SerializeField] private Vector3 closedPosConveyor;
    [SerializeField] private Vector3 openPosConveyor;

    // Speed at which the door moves
    [HideInInspector] public float speed = 1f;
    [HideInInspector] public bool isOpen = false;

    void Start()
    {
        // Set initial positions of the door and conveyor door to closed positions
        door.localPosition = closedPos;
        conveyorDoor.localPosition = closedPosConveyor;
    }

    void FixedUpdate()
    {
        // Calculate door movement speed
        float doorSpeed = speed * Time.fixedDeltaTime;

        // Move the door and conveyor door based on their current state
        if (isOpen)
        {
            door.localPosition = Vector3.Lerp(door.localPosition, openPos, doorSpeed);
            conveyorDoor.localPosition = Vector3.Lerp(conveyorDoor.localPosition, openPosConveyor, doorSpeed);
        }
        else
        {
            door.localPosition = Vector3.Lerp(door.localPosition, closedPos, doorSpeed);
            conveyorDoor.localPosition = Vector3.Lerp(conveyorDoor.localPosition, closedPosConveyor, doorSpeed);
        }
    }

    // Override interaction method from parent class
    public override void interaction()
    {
        if (!canInteract())
            return;

        // If the door is not open, open it
        if (!isOpen)
        {
            isOpen = true;
            // Play door opening sound
            SoundManager.PlaySound(SoundManager.Sound.Door_Open_1, transform.position);
        }
    }
}
