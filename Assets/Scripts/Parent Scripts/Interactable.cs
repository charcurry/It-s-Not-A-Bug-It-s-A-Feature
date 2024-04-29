using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Parent Properties")]
    public float interactionCooldown = 0;
    public bool oneTimeInteraction = false;

    private float cooldownTimeStamp = 0;
    private bool hasBeenInteractedWith = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void interaction()
    {
        
    }

    protected bool canInteract()
    {
        if (cooldownTimeStamp + interactionCooldown > Time.time || hasBeenInteractedWith == true)
            return false;

        if (oneTimeInteraction == true)
            hasBeenInteractedWith = true;

        cooldownTimeStamp = Time.time;

        return true;
    }
}
