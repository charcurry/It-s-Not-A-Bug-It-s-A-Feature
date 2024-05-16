using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailCollisionSounds : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.PlaySound(SoundManager.Sound.Nail_Gun_Hit, transform.position);
    }
}
