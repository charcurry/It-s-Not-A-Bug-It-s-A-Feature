using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [HideInInspector] public bool isObjectHere = false;
    [HideInInspector] public List<Collider> colliderList = new List<Collider>();

    [Header("Properties")]
    [SerializeField] private float triggerThickness;
    [SerializeField] private bool isTopTigger;

    private void Start()
    {
        // Sets the thickness of the trigger independent of scale
        transform.localScale = new Vector3(transform.localScale.x, triggerThickness / transform.parent.transform.localScale.y, transform.localScale.z);

        // Sets Y position to work with new scale
        if (isTopTigger)
            transform.localPosition = new Vector3(0, ((triggerThickness / transform.parent.transform.localScale.y) / 2) + 0.5f, 0);
        else
            transform.localPosition = new Vector3(0, -((triggerThickness / transform.parent.transform.localScale.y) / 2) - 0.5f, 0);
    }

    // Keeps track of all colliders (objects) that enter the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
            colliderList.Add(other);

        if (colliderList.Count > 0)
            isObjectHere = true;
    }

    // Keeps track of all colliders (objects) that exit the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
            colliderList.Remove(other);

        if (colliderList.Count < 1)
            isObjectHere = false;
    }
}
