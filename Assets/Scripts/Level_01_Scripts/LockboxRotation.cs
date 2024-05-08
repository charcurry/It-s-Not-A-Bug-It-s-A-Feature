using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockboxRotation : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        }
        else if (!isPickedUp)
        {
            gameObject.GetComponent<Rigidbody>().freezeRotation = false;
        }
    }
    public override void interaction()
    {
        if (!canInteract())
            return;
    }
}
