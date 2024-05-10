using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVent : MonoBehaviour
{
    private Vector3 gustOrigin;

    [Header("Properties")]
    [SerializeField] private float gustForce = 10;
    [SerializeField] private float airHeight = 10;

    private void Start()
    {
        gameObject.GetComponent<CapsuleCollider>().height = airHeight;
        gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, (airHeight / 2) - 0.3f, 0);

        gustOrigin = transform.parent.transform.position;
    }

    // Get rigidbody component and apply upward force instantly
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
        {
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * Mathf.Clamp(gustForce * (20 - Vector3.Distance(other.transform.position, gustOrigin)), Physics.gravity.y, 20), ForceMode.Acceleration);
        }
    }
}
