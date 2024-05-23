using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.PlaySound(SoundManager.Sound.Key_Collision, transform.position);
    }
}
