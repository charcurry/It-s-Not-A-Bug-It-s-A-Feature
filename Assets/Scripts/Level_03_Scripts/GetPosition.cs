using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPosition : MonoBehaviour
{
    [HideInInspector] public Vector3 initialPosition;
    [HideInInspector] public Quaternion initialRotation;

    [Header("References")]
    public string prefabName;

    //Gets the initial position of any object with this script
    void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
}
