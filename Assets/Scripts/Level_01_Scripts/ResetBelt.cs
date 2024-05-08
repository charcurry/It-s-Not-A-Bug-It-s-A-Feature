using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBelt : Interactable
{
    private ConveyorBelt conveyorBelt;
    private GameObject conveyor;
    // Start is called before the first frame update
    void Start()
    {
        conveyor = GameObject.FindGameObjectWithTag("Conveyor");
        conveyorBelt = conveyor.GetComponent<ConveyorBelt>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void interaction()
    {
        if (!canInteract())
            return;

        foreach (GameObject obj in conveyorBelt.onBelt.ToArray())
        {
            if(obj == obj.CompareTag("Box"))
            {
                Destroy(obj);
            }
        }
    }
}
