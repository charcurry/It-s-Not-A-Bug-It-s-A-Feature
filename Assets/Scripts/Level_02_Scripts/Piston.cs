using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
{
    public float speed = 1.0f;
    public float distance = 10.0f;
    public float pauseTime = 2f;
    public float startDelay = 2f;

    private Vector3 originPosition;
    private int direction = 1;
    private bool isPaused = true;

    void Start()
    {
        originPosition = transform.position;
        StartCoroutine(StartDelay());
    }

    void Update()
    {
        if (!isPaused)
        {
            float newPositionZ = transform.position.z + direction * speed * Time.deltaTime;

            if (direction == 1 && newPositionZ >= originPosition.z + distance)
            {
                newPositionZ = originPosition.z + distance;
                StartCoroutine(Pause());
            }
            else if (direction == -1 && newPositionZ <= originPosition.z)
            {
                newPositionZ = originPosition.z;
                StartCoroutine(Pause());
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, newPositionZ);
        }
    }

    IEnumerator Pause()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseTime);
        direction *= -1;
        isPaused = false;
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        isPaused = false;
    }
}
