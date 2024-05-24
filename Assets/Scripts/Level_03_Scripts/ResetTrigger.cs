using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTrigger : Interactable
{
    [Header("References")]
    [SerializeField] private Reset resetScript;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Rigidbody>())
        {
            resetScript.ResetLevel();
        }
    }

    public override void interaction()
    {
        if (!canInteract())
            return;

        resetScript.ResetLevel();
    }
}
