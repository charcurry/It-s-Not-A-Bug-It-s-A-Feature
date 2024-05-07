using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public Transform door;
    public Vector3 closedPos;
    public Vector3 openPos;
    public float speed = 1f;

    private bool isOpen = false;

    void Start()
    {
        door.localPosition = closedPos;
    }

    void FixedUpdate()
    {
        float doorSpeed = speed * Time.fixedDeltaTime;

        if (isOpen)
        {
            door.localPosition = Vector3.Lerp(door.localPosition, openPos, doorSpeed);
        }
        else
        {
            door.localPosition = Vector3.Lerp(door.localPosition, closedPos, doorSpeed);
        }
    }

    public override void interaction()
    {
        if (!canInteract())
            return;

        if (!isOpen)
        {
            isOpen = true;
        }
    }
}
