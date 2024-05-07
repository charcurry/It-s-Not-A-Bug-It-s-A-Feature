using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    public int[] correctSequence = new int[] { 1, 2, 3, 4, 5 }; // The puzzle order, goes from 1-5
    private int currentIndex = 0;
    public OpenDoor doorScript;
    public Light[] lights; // Array of lights to show progress
    public GameObject[] gameObjects;
    public Material redMaterial;
    public Material greenMaterial;
    private bool puzzleCompleted = false;

    void Start()
    {
        // Set all floor buttons to red material and lights to red color at start
        foreach (GameObject obj in gameObjects)
        {
            obj.GetComponent<Renderer>().material = redMaterial;
        }
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
            // Progress index and change material colors and light colors if right order
            gameObjects[currentIndex].GetComponent<Renderer>().material = greenMaterial;
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

        // Reset all floor buttons to red material and lights to red color, and index to 0 if wrong order
        foreach (GameObject obj in gameObjects)
        {
            obj.GetComponent<Renderer>().material = redMaterial;
        }
        foreach (Light light in lights)
        {
            light.color = Color.red;
        }
        currentIndex = 0;
    }
}
