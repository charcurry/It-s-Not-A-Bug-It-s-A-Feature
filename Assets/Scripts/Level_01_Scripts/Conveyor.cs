using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    //speed is how fast object moves on belt, conveyor speed is how fast the belt material looks
    [SerializeField]
    private float speed, conveyorSpeed;


    //list of objects on conveyor belt
    [SerializeField]
    private List<GameObject> onBelt;

    private Material material;

    //grabs the conveyors material
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    //conveyor material effect
    private void Update()
    {
        material.mainTextureOffset += new Vector2(-1, 0) * conveyorSpeed * Time.deltaTime;
    }

    //physics for objects on belt
    void FixedUpdate()
    {
        foreach (GameObject obj in onBelt.ToArray())
        {
            if (obj != null)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 movement = speed * transform.right * Time.fixedDeltaTime;
                    rb.MovePosition(rb.position + movement);
                    rb.freezeRotation = true;
                }
                else
                {
                    Debug.LogWarning("Rigidbody component missing on object: " + obj.name);
                }
            }
            else
            {
                onBelt.Remove(obj);
            }
        }
    }

    //object enters conveyor belt
    private void OnCollisionEnter(Collision collision)
    {
        onBelt.Add(collision.gameObject);
    }

    //object exits conveyor belt
    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        onBelt.Remove(collision.gameObject);
        if (collision.gameObject.CompareTag("Box"))
        {
            rb.freezeRotation = false;
        }
    }
}