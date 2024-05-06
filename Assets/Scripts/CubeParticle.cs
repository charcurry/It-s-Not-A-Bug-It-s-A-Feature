using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeParticle : Interactable
{
    public ParticleSystem particles;

    public override void interaction()
    {
        if (!canInteract())
            return;
        Debug.Log("Hit");
        particles.Stop();
        particles.Play();
    }
}
