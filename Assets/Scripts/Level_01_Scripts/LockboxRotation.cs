using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockboxRotation : Interactable
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FreezeRotation()
    {
        while (isPickedUp)
        {
            rb.freezeRotation = true;
            yield return null;
        }
        rb.freezeRotation = false; // Unfreeze rotation when not picked up
    }

    public override void interaction()
    {
        if (!canInteract())
            return;

        isPickedUp = !isPickedUp; // Toggle the value of isPickedUp

        if (isPickedUp)
        {
            StartCoroutine(FreezeRotation()); // Start freezing rotation if picked up
        }
    }
}
