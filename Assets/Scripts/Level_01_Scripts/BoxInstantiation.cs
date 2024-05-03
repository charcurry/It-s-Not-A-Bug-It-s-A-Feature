using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInstantiation : MonoBehaviour
{
    public GameObject boxPrefab;
    public float instantiationInterval = 2f;
    private float timer;
    public int boxCount = 0;
    private int maxBoxLimit = 30;

    void Start()
    {
        timer = instantiationInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // If the timer reaches zero or less and box count is less than the limit, instantiate the prefab and reset the timer
        if (timer <= 0f && boxCount < maxBoxLimit)
        {
            Instantiate(boxPrefab, transform.position, Quaternion.identity);
            timer = instantiationInterval;
            boxCount++; //increase the box count
        }
    }
}