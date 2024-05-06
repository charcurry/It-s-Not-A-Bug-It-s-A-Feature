using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public int triggerId;
    private SequenceManager sequenceManager;

    void Start()
    {
        // Find sequence manager in level
        sequenceManager = FindObjectOfType<SequenceManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify sequence manager
            sequenceManager.TriggerEntered(triggerId);
        }
    }
}
