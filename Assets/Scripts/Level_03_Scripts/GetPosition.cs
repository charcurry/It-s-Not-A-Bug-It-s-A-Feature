using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPosition : MonoBehaviour
{
    public Vector3 initialPosition;
    public Quaternion initialRotation;

    //Gets the initial position of any object with this script
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
}
