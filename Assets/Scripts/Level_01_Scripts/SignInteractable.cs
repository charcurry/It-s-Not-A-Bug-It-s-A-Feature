using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SignInteractable : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void interaction()
    {
        if (!canInteract())
            return;
        isPickedUp = !isPickedUp;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
    }
}
