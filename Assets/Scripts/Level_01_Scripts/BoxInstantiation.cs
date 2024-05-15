using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInstantiation : MonoBehaviour
{
    private ConveyorBelt conveyorBelt;
    public GameObject conveyor;
    public GameObject boxPrefab;
    public Transform targetArea; // Assuming target area is represented by a transform
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

        // If the timer reaches zero or less and box count is less than the limit, and there's no box in the target area, instantiate the prefab and reset the timer
        if (timer <= 0f && conveyorBelt.objectsOnBelt.Count < boxCount && !IsBoxInTargetArea())
        {
            Instantiate(boxPrefab, transform.position, Quaternion.identity);
            timer = instantiationInterval;
        }
    }

    bool IsBoxInTargetArea()
    {
        Collider[] colliders = Physics.OverlapBox(targetArea.position, targetArea.localScale / 2);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Box")) // Assuming the box has a tag "Box"
            {
                return true;
            }
        }
        return false;
    }
}
