using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBox : Interactable
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void interaction()
    {
        if (!canInteract())
            return;

    }
}
