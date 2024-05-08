using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInstantiation : MonoBehaviour
{
    private ConveyorBelt conveyorBelt;
    private GameObject conveyor;
    public GameObject boxPrefab;
    public float instantiationInterval = 2f;
    private float timer;
    public int boxCount = 0;

    void Start()
    {
        conveyor = GameObject.FindGameObjectWithTag("Conveyor");
        conveyorBelt = conveyor.GetComponent<ConveyorBelt>();
        timer = instantiationInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // If the timer reaches zero or less and box count is less than the limit, instantiate the prefab and reset the timer
        if (timer <= 0f && conveyorBelt.onBelt.Count < 23)
        {
            Instantiate(boxPrefab, transform.position, Quaternion.identity);
            timer = instantiationInterval;
        }
    }
}