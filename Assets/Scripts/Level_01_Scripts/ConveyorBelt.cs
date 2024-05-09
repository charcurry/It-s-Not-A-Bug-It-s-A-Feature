using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    public int boxCount;
    public List<Rigidbody> objectsOnBelt = new List<Rigidbody>();
    public List<Renderer> beltRenderers = new List<Renderer>();

    private void Start()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (renderer.CompareTag("Belt"))
            {
                beltRenderers.Add(renderer);
            }
        }
    }

    public void MoveBelt(float delta)
    {
        foreach (Rigidbody objRb in objectsOnBelt)
        {
            if (objRb != null)
            {
                Vector3 movement = speed * transform.right * delta;
                objRb.MovePosition(objRb.position + movement);
                objRb.freezeRotation = true;
            }
            else
            {
                objectsOnBelt.Remove(objRb);
            }
        }
    }

    public void MoveMaterial(float delta)
    {
        foreach (Renderer renderer in beltRenderers)
        {
            foreach (Material material in renderer.materials)
            {
                material.mainTextureOffset += new Vector2(-1, 0) * delta;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
            objectsOnBelt.Add(collision.gameObject.GetComponent<Rigidbody>());
    }

    private void OnCollisionExit(Collision collision)
    {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            objectsOnBelt.Remove(rb);
        if (collision.gameObject.CompareTag("Box"))
        {
            rb.freezeRotation = false;
        }
    }

    public void StopMaterialMovement()
    {
        foreach (Renderer renderer in beltRenderers)
        {
            foreach (Material material in renderer.materials)
            {
                material.mainTextureOffset = Vector2.zero;
            }
        }
    }
}