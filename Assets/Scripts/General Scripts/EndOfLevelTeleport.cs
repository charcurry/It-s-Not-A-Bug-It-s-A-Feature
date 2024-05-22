using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevelTeleport : MonoBehaviour
{
    // If the player enters this trigger, go to next level
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            NarratorManager.get.TriggerHappened("start");
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            NarratorManager.get.TriggerHappened("start3");
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            NarratorManager.get.TriggerHappened("start4");
        }
    }
}
