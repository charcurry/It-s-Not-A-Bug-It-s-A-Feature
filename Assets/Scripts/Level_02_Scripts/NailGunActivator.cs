using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailGunActivator : Interactable
{
    [SerializeField] private NailGun nailGun;

    // Picking up the nail gun
    public override void interaction()
    {
        if (canInteract())
        {
            nailGun.EnableNailGun();
            DeactivateObject();
        }
    }
}
