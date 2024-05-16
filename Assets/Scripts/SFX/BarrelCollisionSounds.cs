using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCollisionSounds : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.PlaySound(SoundManager.Sound.Barrel_Collision, transform.position);
    }
}
