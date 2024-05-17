using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public float speed = 3.0f;
    public float distance = 5.0f;
    private float moveDistance = 0.0f;

    public void Open()
    {
        StopAllCoroutines();
        StartCoroutine(OpenRoutine());
    }

    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(CloseRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        SoundManager.PlaySound(SoundManager.Sound.Door_Open_2, transform.position);
        while (moveDistance < distance)
        {
            float moveStep = speed * Time.deltaTime;
            moveDistance += moveStep;
            transform.Translate(0, moveStep, 0);
            yield return null;
        }
    }

    private IEnumerator CloseRoutine()
    {
        SoundManager.PlaySound(SoundManager.Sound.Door_Open_2, transform.position);
        while (moveDistance > 0)
        {
            float moveStep = speed * Time.deltaTime;
            moveDistance -= moveStep;
            transform.Translate(0, -moveStep, 0);
            yield return null;
        }
    }
}
