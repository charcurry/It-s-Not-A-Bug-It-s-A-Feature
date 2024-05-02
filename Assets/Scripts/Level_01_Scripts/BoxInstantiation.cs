using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInstantiation : MonoBehaviour
{
    public GameObject boxPrefab;
    public float instantiationInterval = 2f;
    private float timer;

    void Start()
    {
        timer = instantiationInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        //if the timer reaches zero or less, instantiate the prefab and reset the timer
        if (timer <= 0f)
        {
            Instantiate(boxPrefab, transform.position, Quaternion.identity);
            timer = instantiationInterval;
        }
    }
}
