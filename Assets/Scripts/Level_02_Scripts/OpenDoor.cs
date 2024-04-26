using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    // This script only opens the door. No need to close it once puzzle is completed

    public float speed = 3.0f;
    public float distance = 5.0f;
    private bool isOpening = false;
    private float moveDistance = 0.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isOpening) // For testing
        {
            isOpening = true;
        }

        if (isOpening && moveDistance < distance)
        {
            float totalTime = speed * Time.deltaTime;
            if (moveDistance + totalTime > distance)
                totalTime = distance - moveDistance;

            transform.Translate(0, totalTime, 0);
            moveDistance += totalTime;
        }
        else if (moveDistance >= distance)
        {
            isOpening = false;
        }
    }
}
