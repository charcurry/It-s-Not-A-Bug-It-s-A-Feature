using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInstantiation : MonoBehaviour
{
    private ConveyorBelt conveyorBelt;
    public GameObject conveyor;
    public GameObject boxPrefab;
    public float instantiationInterval = 2f;
    private float timer;
    public int boxCount;

    void Start()
    {
        conveyorBelt = conveyor.GetComponent<ConveyorBelt>();
        timer = instantiationInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // If the timer reaches zero or less and box count is less than the limit, instantiate the prefab and reset the timer
        if (timer <= 0f && conveyorBelt.objectsOnBelt.Count != boxCount)
        {
            Instantiate(boxPrefab, transform.position, Quaternion.identity);
            timer = instantiationInterval;
        }
    }
}