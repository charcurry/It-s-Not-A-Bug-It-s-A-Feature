using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    public int[] correctSequence = new int[] { 1, 2, 3, 4, 5 }; // The puzzle order, goes from 1-5
    private int currentIndex = 0;
    public OpenDoor doorScript;

    public void TriggerEntered(int triggerId)
    {
        if (currentIndex < correctSequence.Length && triggerId == correctSequence[currentIndex])
        {
            // Progress index if right order
            currentIndex++;
            if (currentIndex == correctSequence.Length)
            {
                doorScript.Open();
            }
        }
        else
        {
            currentIndex = 0; // Reset sequence if wrong order
        }
    }
}
