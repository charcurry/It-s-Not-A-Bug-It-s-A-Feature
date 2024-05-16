using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int[] correctSequence = new int[] { 1, 2, 3, 4, 5 }; // The puzzle order

    [Header("References")]
    [SerializeField] private OpenDoor doorScript;
    [SerializeField] private Light[] lights;
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material destroyedMaterial;

    private int currentIndex = 0;
    private bool puzzleCompleted = false;

    void Start()
    {
        // Set all buttons and lights to inactive state
        SetButtonMaterial(redMaterial);
        SetLightsColor(Color.red);
    }

    public void TriggerEntered(int triggerId)
    {
        // Ignore if puzzle is already completed
        if (puzzleCompleted)
            return;

        // Check if the entered trigger is correct
        if (CheckTrigger(triggerId))
        {
            HandleCorrectOrder();
        }
        else
        {
            ResetSequence();
        }
    }

    // Check if the id matches the order
    private bool CheckTrigger(int triggerId)
    {
        return currentIndex < correctSequence.Length && triggerId == correctSequence[currentIndex];
    }

    // Handle if correct order
    private void HandleCorrectOrder()
    {
        // Update button material and light color for the current index
        UpdateMatAndLights(currentIndex, greenMaterial, Color.green);
        SoundManager.PlaySound(SoundManager.Sound.Correct_Sound, gameObjects[currentIndex].transform.position);
        currentIndex++;

        // Check if the puzzle is completed
        if (currentIndex == correctSequence.Length)
        {
            CompletePuzzle();
        }
    }

    // Complete the puzzle
    private void CompletePuzzle()
    {
        puzzleCompleted = true;
        SoundManager.PlaySound(SoundManager.Sound.Puzzle_Solved);
        doorScript.Open();
    }

    // Reset the puzzle to its start state
    private void ResetSequence()
    {
        if (puzzleCompleted)
            return;

        // Reset all buttons and lights
        SetButtonMaterial(redMaterial);
        SetLightsColor(Color.red);
        SoundManager.PlaySound(SoundManager.Sound.Incorrect_Sound);
        currentIndex = 0;
    }

    // Change button materials when electrical panel is damaged
    public void SetToDisabledMaterial()
    {
        SetButtonMaterial(destroyedMaterial);
    }

    // Set material for all floor buttons
    private void SetButtonMaterial(Material material)
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.GetComponent<Renderer>().material = material;
        }
    }

    // Set color for all lights
    private void SetLightsColor(Color color)
    {
        foreach (Light light in lights)
        {
            light.color = color;
        }
    }

    // Update material and light color for each button
    private void UpdateMatAndLights(int index, Material material, Color color)
    {
        gameObjects[index].GetComponent<Renderer>().material = material;
        lights[index].color = color;
    }
}
