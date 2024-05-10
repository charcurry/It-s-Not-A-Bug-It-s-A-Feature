using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startPoint;
    private Vector3 endPoint;
    private PlatformTrigger topTrigger;
    private PlatformTrigger bottomTrigger;
    private Vector3 currentDestination;
    private Vector3 previousPosition;
    private Vector3 platformPositionDelta;
    private float timeStamp;

    [Header("Properties")]
    [SerializeField] private float platformWaitTime;
    [SerializeField] private float speed;

    void Start()
    {
        SoundManager.PlaySound(SoundManager.Sound.Moving_Platform, transform.position, gameObject);
        startPoint = transform.parent.GetChild(1).transform.position;
        endPoint = transform.parent.GetChild(2).transform.position;

        topTrigger = transform.GetChild(0).GetComponent<PlatformTrigger>();
        bottomTrigger = transform.GetChild(1).GetComponent<PlatformTrigger>();

        transform.position = startPoint;
        currentDestination = endPoint;

        timeStamp = Time.time - platformWaitTime;
    }

    
    void Update()
    {

        Vector3 previousCurrentDestination = currentDestination;

        // Keeps track of weather or not the platform has reached its destination
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

        // Moves the platfrom towards the current destination
        if (timeStamp + platformWaitTime <= Time.time && !bottomTrigger.isObjectHere)
            transform.Translate((currentDestination - transform.position).normalized * speed * Time.fixedDeltaTime);

        platformPositionDelta = transform.position - previousPosition;

        // Adds the delta of the last platform movement to each object on top of it
        foreach (Collider collider in topTrigger.colliderList)
            collider.transform.position += platformPositionDelta;
    }
}
