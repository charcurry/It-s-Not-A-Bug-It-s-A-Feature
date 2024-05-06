using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    public int[] correctSequence = new int[] { 1, 2, 3, 4, 5 }; // The puzzle order, goes from 1-5
    private int currentIndex = 0;
    public OpenDoor doorScript;
    public Light[] lights; // Array of lights to show progress
    private bool puzzleCompleted = false;

    void Start()
    {
        // Make lights red at start incase they are somehow a different color
        foreach (Light light in lights)
        {
            light.color = Color.red;
        }
    }

    public void TriggerEntered(int triggerId)
    {
        if (puzzleCompleted) return; // Ignore triggers if the puzzle is already completed

        if (currentIndex < correctSequence.Length && triggerId == correctSequence[currentIndex])
        {
            // Progress index and change light colors if right order
            lights[currentIndex].color = Color.green;
            currentIndex++;
            if (currentIndex == correctSequence.Length)
            {
                puzzleCompleted = true;
                doorScript.Open();
            }
        }
        else
        {
            ResetSequence();
        }
    }

    void ResetSequence()
    {
        if (puzzleCompleted) return;

        // Reset all lights to red and index to 0 if wrong order
        foreach (Light light in lights)
        {
            light.color = Color.red;
        }
        currentIndex = 0;
    }
}
