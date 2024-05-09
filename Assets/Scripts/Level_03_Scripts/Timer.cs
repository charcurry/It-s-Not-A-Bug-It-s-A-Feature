using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float maxTime = 90f;
    private float currentTime;
    private bool timerEnded = false;

    void Start()
    {
        currentTime = maxTime;
    }

    void Update()
    {

        if (!timerEnded)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                Reset resetScript = FindObjectOfType<Reset>();
                if (resetScript != null)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    timerEnded = true;
                }
                else
                {
                    Debug.LogError("Reset script not found!");
                }
                currentTime = maxTime;
            }
            UpdateTimerDisplay();
        }

    }

    void UpdateTimerDisplay()
    {
        int seconds = Mathf.FloorToInt(currentTime);
        timerText.text = string.Format("Time: {0}", seconds);
    }



}

