using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    private void OnCollisionEnter(Collision collision)
    {
        if (CompareTag("KeyCard"))
        {
            SoundManager.PlaySound(SoundManager.Sound.Key_Card_Collision, transform.position);
        }
        else
        {
            SoundManager.PlaySound(SoundManager.Sound.Key_Collision, transform.position);
        }
    }
}
