using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Interactable
{
    // Plays a box collision sound whenever the box colliders with something
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Conveyor"))
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
