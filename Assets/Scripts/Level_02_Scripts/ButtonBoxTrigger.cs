using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBoxTrigger : MonoBehaviour
{
    public Material switchMaterial;
    public Material defaultMaterial;
    public GameObject floorButton;
    public Light lightObject;
    public OpenDoor doorScript;

    private bool isTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("ButtonBox"))
        {
            isTriggered = true;
            SoundManager.PlaySound(SoundManager.Sound.Puzzle_Solved);
            floorButton.GetComponent<Renderer>().material = switchMaterial;
            lightObject.color = Color.green;
            doorScript.Open();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isTriggered && other.CompareTag("ButtonBox"))
        {
            isTriggered = false;
            SoundManager.PlaySound(SoundManager.Sound.Incorrect_Sound);
            floorButton.GetComponent<Renderer>().material = defaultMaterial;
            lightObject.color = Color.red;
            doorScript.Close();
        }
    }
}
