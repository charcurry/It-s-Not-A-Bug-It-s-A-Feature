using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public Transform door;
    public Transform conveyorDoor;
    public Vector3 closedPos;
    public Vector3 openPos;
    public Vector3 closedPosConveyor;
    public Vector3 openPosConveyor;
    public float speed = 1f;

    private bool isOpen = false;

    void Start()
    {
        door.localPosition = closedPos;
        conveyorDoor.localPosition = closedPosConveyor;
    }

    void FixedUpdate()
    {

        float doorSpeed = speed * Time.fixedDeltaTime;

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

    public override void interaction()
    {
        if (!canInteract())
            return;

        if (!isOpen)
        {
            isOpen = true;
            SoundManager.PlaySound(SoundManager.Sound.Door_Open_1, transform.position);
        }
    }
}
