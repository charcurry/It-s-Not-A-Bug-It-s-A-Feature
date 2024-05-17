using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OpenDoor doorScript;
    [SerializeField] private Material switchMaterial;
    [SerializeField] private Material defaultMaterial;

    private bool isTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("ButtonBox"))
        {
            isTriggered = true;
            SoundManager.PlaySound(SoundManager.Sound.Puzzle_Solved);
            gameObject.GetComponent<Renderer>().material = switchMaterial;
            doorScript.Open();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isTriggered && other.CompareTag("ButtonBox"))
        {
            isTriggered = false;
            SoundManager.PlaySound(SoundManager.Sound.Incorrect_Sound);
            gameObject.GetComponent<Renderer>().material = defaultMaterial;
            doorScript.Close();
        }
    }
}
