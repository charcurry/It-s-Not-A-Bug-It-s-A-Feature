using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Parent Properties")]
    public bool pickupable = false;
    [SerializeField] private float interactionCooldown = 0;
    [SerializeField] private bool oneTimeInteraction = false;

    private float cooldownTimeStamp = 0;
    private bool hasBeenInteractedWith = false;

    [HideInInspector] public bool isPickedUp;

    public virtual void interaction()
    {
        
    }

    private void Awake()
    {
        if (gameObject.GetComponent<Rigidbody>())
            gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void DeactivateObject()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        if (gameObject.GetComponent<Collider>() != null)
            gameObject.GetComponent<Collider>().enabled = false;

        if (gameObject.GetComponent<MeshRenderer>() != null)
            gameObject.GetComponent<MeshRenderer>().enabled = false;

        pickupable = false;
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
