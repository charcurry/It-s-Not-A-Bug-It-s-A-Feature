using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    //speed is how fast object moves on belt, conveyor speed is how fast the belt material looks
    [SerializeField]
    private float speed, conveyorSpeed;

    //what direction you want the belt to go
    [SerializeField]
    private Vector3 direction;

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
        for (int i = 0; i <= onBelt.Count - 1; i++)
        {
            onBelt[i].GetComponent<Rigidbody>().AddForce(speed * direction);
            onBelt[i].GetComponent<Rigidbody>().freezeRotation = true;
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
        onBelt.Remove(collision.gameObject);
    }
}