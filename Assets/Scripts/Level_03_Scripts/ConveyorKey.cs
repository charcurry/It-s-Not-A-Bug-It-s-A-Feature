using System.Collections.Generic;
using UnityEngine;

public class ConveyorKey : MonoBehaviour
{
    private float speed = 1f;
    [HideInInspector] public List<Rigidbody> objectsOnBelt = new List<Rigidbody>();
    [HideInInspector] public List<Renderer> beltRenderers = new List<Renderer>();

    [Header("Properties")]
    public float conveyorSpeed;
    public float objectSpeed;

    private void Start()
    {
        // Get all renderers in children and add them to beltRenderers list
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (renderer.CompareTag("Belt"))
            {
                beltRenderers.Add(renderer);
            }
        }
    }


    private void FixedUpdate()
    {
        // Move the belt and its material texture based on specified speeds and fixed delta time
        MoveBelt(objectSpeed * Time.fixedDeltaTime);
        MoveMaterial(conveyorSpeed * Time.fixedDeltaTime);
    }

    public void MoveBelt(float delta)
    {
        foreach (Rigidbody objRb in objectsOnBelt.ToArray())
        {
            if (objRb != null)
            {
                // Calculate movement based on speed and delta time
                Vector3 movement = speed * transform.right * delta;
                // Move the Rigidbody
                objRb.MovePosition(objRb.position + movement);
                // Freeze rotation to prevent objects from rotating on the belt
                objRb.freezeRotation = true;
            }
            else
            {
                objectsOnBelt.Remove(objRb);
            }
        }
    }

    // Moves the material texture on the belt
    public void MoveMaterial(float delta)
    {
        foreach (Renderer renderer in beltRenderers)
        {
            foreach (Material material in renderer.materials)
            {
                // Move the material texture offset to create the illusion of movement
                material.mainTextureOffset += new Vector2(-1, 0) * delta;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the colliding object has a Rigidbody, add it to the objectsOnBelt list
        objectsOnBelt.Add(collision.gameObject.GetComponent<Rigidbody>());
    }

    private void OnCollisionExit(Collision collision)
    {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            objectsOnBelt.Remove(rb);

        // Allow box to rotate after its removed from belt
        if (collision.gameObject.CompareTag("Box"))
        {
            rb.freezeRotation = false;
        }
    }

    // Stops the movement of material texture on the belt
    public void StopMaterialMovement()
    {
        foreach (Renderer renderer in beltRenderers)
        {
            foreach (Material material in renderer.materials)
            {
                // Reset the material texture offset to stop movement
                material.mainTextureOffset = Vector2.zero;
            }
        }
    }
}