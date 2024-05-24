using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private bool isFast = true;

    // Update is called once per frame
    void Update()
    {
        if (isFast)
            transform.Rotate(Vector3.up * 500 * Time.deltaTime);
        else
            transform.Rotate(Vector3.up * 200 * Time.deltaTime);
    }
}
