using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Parent Properties")]
    public bool pickupable = false;
    [SerializeField] private float interactionCooldown = 0;
    [SerializeField] private bool oneTimeInteraction = false;

    private float cooldownTimeStamp = 0;
    private bool hasBeenInteractedWith = false;

    [HideInInspector] public bool isDeactivated;
    [HideInInspector] public bool isPickedUp;

    private void Awake()
    {
        isDeactivated = false;

        if (gameObject.GetComponent<Rigidbody>())
        {
            gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        }
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
        isDeactivated = true;
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

    public virtual void interaction()
    {
        
    }
}
