using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBox : Explodable
{
    public override void Explode()
    {
        transform.parent.GetChild(1).GetComponent<ParticleSystem>().Play();
        SoundManager.PlaySound(SoundManager.Sound.Glass_Shattering, transform.position);
        gameObject.SetActive(false);
    }
}
