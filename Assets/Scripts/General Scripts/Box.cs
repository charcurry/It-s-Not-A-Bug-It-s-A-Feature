using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Interactable
{
    private bool canPlaySound = false;

    private void Start()
    {
        // Start the coroutine to enable sound after a delay
        StartCoroutine(EnableSoundAfterDelay(1f));
    }

    private IEnumerator EnableSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPlaySound = true;
    }

    // Plays a box collision sound whenever the box collides with something, after the delay
    private void OnCollisionEnter(Collision collision)
    {
        if (canPlaySound && !collision.gameObject.CompareTag("Conveyor"))
        {
            SoundManager.PlaySound(SoundManager.Sound.Box_Collision, transform.position);
        }
    }

    // Plays the box destory particle effect before being destroyed
    public void DestroyBox()
    {
        if (transform.GetChild(0).GetComponent<ParticleSystem>())
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        // ADD SOUND HERE

        DeactivateObject();
    }
}
