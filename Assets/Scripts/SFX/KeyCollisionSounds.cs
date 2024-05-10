using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollisionSounds : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.PlaySound(SoundManager.Sound.Key_Collision, transform.position);
    }
}
