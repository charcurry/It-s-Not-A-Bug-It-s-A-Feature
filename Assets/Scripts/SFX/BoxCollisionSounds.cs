using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollisionSounds : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Conveyor"))
        {
            SoundManager.PlaySound(SoundManager.Sound.Box_Collision, transform.position);
        }
    }
}
