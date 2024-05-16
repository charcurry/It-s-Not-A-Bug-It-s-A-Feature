using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInstantiation : MonoBehaviour
{
    private ConveyorBelt conveyorBelt;
    public GameObject conveyor;
    public GameObject boxPrefab;
    public Transform targetArea;
    public float instantiationInterval = 2f;
    private float timer;
    public int boxCount;

    void Start()
    {
        // Get the ConveyorBelt component from the conveyor GameObject
        conveyorBelt = conveyor.GetComponent<ConveyorBelt>();

        // Initialize the timer
        timer = instantiationInterval;
    }

    void Update()
    {
        // Decrease the timer by the time passed since the last frame
        timer -= Time.deltaTime;

        // If the timer reaches zero or less, and the box count is less than the limit, and there's no box in the target area, instantiate a new box and reset the timer
        if (timer <= 0f && conveyorBelt.objectsOnBelt.Count < boxCount && !IsBoxInTargetArea())
        {
            Instantiate(boxPrefab, transform.position, Quaternion.identity);
            timer = instantiationInterval;
        }
    }

    // Check if there is a box in the target area
    bool IsBoxInTargetArea()
    {
        // OverlapBox to check for colliders in the target area
        Collider[] colliders = Physics.OverlapBox(targetArea.position, targetArea.localScale / 2);
        foreach (Collider collider in colliders)
        {
            // If a box collider is found, return true
            if (collider.CompareTag("Box"))
            {
                return true;
            }
        }
        // Return false if no box collider is found
        return false;
    }
}
