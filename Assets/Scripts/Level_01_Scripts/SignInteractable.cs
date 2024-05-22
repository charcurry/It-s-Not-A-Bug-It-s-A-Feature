using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SignInteractable : Interactable
{
    public bool wasInteracted = false;
    public override void interaction()
    {
        if (!canInteract())
            return;
        isPickedUp = !isPickedUp;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
    }

    public void Update()
    {
        if (isPickedUp)
        {
            wasInteracted = true;
        }
        if (wasInteracted)
        {
            NarratorManager.get.TriggerHappened("signInteracted");
        }
    }
}
