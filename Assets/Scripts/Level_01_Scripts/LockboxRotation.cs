using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBoxRotation : Interactable
{
    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Coroutine to freeze rotation while the box is picked up
    IEnumerator FreezeRotation()
    {
        // Loop as long as the box is picked up
        while (isPickedUp)
        {
            // Freeze rotation of the Rigidbody to prevent it from rotating
            rb.freezeRotation = true;
            // Wait for the next frame
            yield return null;
        }
        // Once the box is no longer picked up allow rotation
        rb.freezeRotation = false;
    }

    public override void interaction()
    {
        if (!canInteract())
            return;

        isPickedUp = !isPickedUp;

        // If the box is picked up start the coroutine to freeze rotation
        if (isPickedUp)
        {
            StartCoroutine(FreezeRotation());
        }
    }
}
