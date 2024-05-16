using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Lock> lockList;

    [HideInInspector] public bool isOpening;
    private Quaternion lerpTarget;
    float timeCount;

    private void Start()
    {
        isOpening = false;
        lerpTarget = Quaternion.Euler(0, -80, 0);
        timeCount = 0.0f;
    }

    private void Update()
    {
        foreach (Lock padlock in lockList)
        {
            if (padlock.isUnlocked)
            {
                lockList.Remove(padlock);
                break;
            }
        }

        if (lockList.Count == 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, lerpTarget, timeCount * 0.01f);
            timeCount += Time.deltaTime;
        }
    }
}
