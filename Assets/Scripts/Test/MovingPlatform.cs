using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 currentDestination;
    private Vector3 previousPosition;
    private Vector3 platformPositionDelta;
    private float timeStamp;

    [Header("Properties")]
    [SerializeField] private float platformWaitTime;
    [SerializeField] private float speed;

    void Start()
    {
        startPoint = transform.parent.GetChild(1).transform.position;
        endPoint = transform.parent.GetChild(2).transform.position;

        transform.position = startPoint;
        currentDestination = endPoint;

        timeStamp = Time.time - platformWaitTime;
    }

    void Update()
    {
        Vector3 previousCurrentDestination = currentDestination;

        if (Vector3.Distance(transform.position, startPoint) <= 0.1f)
            currentDestination = endPoint;

        if (Vector3.Distance(transform.position, endPoint) <= 0.1f)
            currentDestination = startPoint;

        if (previousCurrentDestination != currentDestination)
            timeStamp = Time.time;
    }

    private void FixedUpdate()
    {
        previousPosition = transform.position;

        if (timeStamp + platformWaitTime <= Time.time)
            transform.Translate((currentDestination - transform.position).normalized * speed * Time.fixedDeltaTime);

        platformPositionDelta = transform.position - previousPosition;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position += platformPositionDelta;
        }
    }
}
